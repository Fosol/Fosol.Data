using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models.Configuration
{
    public class DataModelElement
        : ConfigurationElement
    {
        #region Variables
        private DbConnection _Connection;
        #endregion

        #region Properties
        /// <summary>
        /// get/set - A unique name to identify this datamodel.
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// get/set - An alias to use to identify this datamodel.
        /// </summary>
        [ConfigurationProperty("alias", IsRequired = false)]
        public string Alias
        {
            get { return (string)this["alias"]; }
            set { this["alias"] = value; }
        }

        /// <summary>
        /// get/set - The namespace to use when creating the datamodel.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Namespace cannot be null or empty")]
        [ConfigurationProperty("namespace", IsRequired = false, DefaultValue = "Model")]
        public string Namespace
        {
            get { return (string)this["namespace"]; }
            set 
            {
                Fosol.Common.Validation.Assert.IsNotNullOrEmpty(value, "Namespace");
                this["namespace"] = value; 
            }
        }

        /// <summary>
        /// get/set - Whether to use Fluent API when creating the code for the model.
        /// </summary>
        [ConfigurationProperty("useFluentApi", IsRequired = false, DefaultValue = false)]
        public bool UseFluentApi
        {
            get { return (bool)this["useFluentApi"]; }
            set { this["useFluentApi"] = value; }
        }

        /// <summary>
        /// get/set - The data provider type name for the datasource.
        /// </summary>
        [ConfigurationProperty("providerName", IsRequired = false)]
        public string ProviderName
        {
            get { return (string)this["providerName"]; }
            set { this["providerName"] = value; }
        }

        /// <summary>
        /// get/set - The connection string for the datasource.
        /// </summary>
        [ConfigurationProperty("connectionString", IsRequired = false)]
        public string ConnectionString
        {
            get { return (string)this["connectionString"]; }
            set { this["connectionString"] = value; }
        }

        /// <summary>
        /// get/set - Datamodel convention to follow when building.
        /// </summary>
        [ConfigurationProperty("convention", IsRequired = false)]
        public ConventionElement Convention
        {
            get { return (ConventionElement)this["convention"]; }
            set { this["convention"] = value; }
        }

        /// <summary>
        /// get/set - Collection of tables from the datasource.
        /// </summary>
        [ConfigurationProperty("tables", IsRequired = false)]
        public TableElementCollection Tables
        {
            get { return (TableElementCollection)this["tables"]; }
            private set { this["tables"] = value; }
        }

        /// <summary>
        /// get/set - Collection of views from the datasource.
        /// </summary>
        [ConfigurationProperty("views", IsRequired = false)]
        public ViewElementCollection Views
        {
            get { return (ViewElementCollection)this["views"]; }
            private set { this["views"] = value; }
        }

        /// <summary>
        /// get/set - Collection of routines from the datasource.
        /// </summary>
        [ConfigurationProperty("routines", IsRequired = false)]
        public RoutineElementCollection Routines
        {
            get { return (RoutineElementCollection)this["routines"]; }
            private set { this["routines"] = value; }
        }

        /// <summary>
        /// get - A DbConnection object this model will use to connect to the database.  The type is determined by the ProviderName property.
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                if (string.IsNullOrEmpty(this.ConnectionString))
                    return null;

                if (_Connection != null)
                    return _Connection;

                // Check if the ConnectionString value is a name from the ConnectionString configuration section.
                var cs = System.Configuration.ConfigurationManager.ConnectionStrings[this.ConnectionString];

                if (cs != null)
                {
                    var factory = DbProviderFactories.GetFactory(cs.ProviderName);
                    _Connection = factory.CreateConnection();
                    _Connection.ConnectionString = cs.ConnectionString;
                    return _Connection;
                }
                else
                {
                    // Create a new Connection object for the specified provider.
                    var factory = DbProviderFactories.GetFactory(this.ProviderName);
                    _Connection = factory.CreateConnection();
                    _Connection.ConnectionString = this.ConnectionString;
                    return _Connection;
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a DataModelElement object.
        /// </summary>
        public DataModelElement()
        {
        }

        /// <summary>
        /// Creates a new instance of a DataModelElement object.
        /// </summary>
        /// <param name="name">Unique name to identify this datamodel.</param>
        public DataModelElement(string name, string alias = null, string modelNamespace = null, string providerName = null, string connectionString = null)
            : this()
        {
            this.Name = name;
            this.Alias = alias;
            this.Namespace = modelNamespace;
            this.ProviderName = providerName;
            this.ConnectionString = connectionString;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Updates the model based on the configuration settings.
        /// </summary>
        /// <param name="model">Model object to apply configuration settings to.</param>
        public void ApplyConfiguration(Model model)
        {
            model.Alias = this.Convention.CreateAlias(model.Name);

            ApplyConfigurationToTables(model);

            ApplyConfigurationToViews(model);

            ApplyConfigurationToRoutines(model);
        }

        private void ApplyConfigurationToTables(Model model)
        {
            var remove_entities = new List<string>();

            foreach (var table in model.Tables)
            {
                var table_config = this.Tables.FirstOrDefault(t => t.Name.Equals(table.Name, StringComparison.InvariantCultureIgnoreCase));

                // This table is not a part of the configuration, it should be removed from the model.
                if ((table_config == null 
                    && this.Tables.Import == ImportOption.Configured)
                    || (table_config != null
                    && table_config.Action == ImportAction.Ignore))
                {
                    remove_entities.Add(table.Name);
                    continue;
                }

                if (table_config == null)
                    table_config = (TableElement)table;

                table.Alias = this.Convention.CreateAlias(table.Name);

                ApplyConfigurationToColumns(table_config, table);

                ApplyConfigurationToConstraints(table_config, table);
            }

            // Remove tables from model that are not part of the configuration.
            foreach (var name in remove_entities)
            {
                model.Tables.Remove(name);
            }
        }

        private void ApplyConfigurationToViews(Model model)
        {
            var view_import_option = this.Views.Import;
            var remove_entities = new List<string>();

            foreach (var view in model.Views)
            {
                var config = this.Views.FirstOrDefault(v => v.Name.Equals(view.Name, StringComparison.InvariantCultureIgnoreCase));

                // This view is not a part of the configuration, it should be removed from the model.
                if ((config == null 
                    && view_import_option == ImportOption.Configured)
                    || (config != null
                    && config.Action == ImportAction.Ignore))
                {
                    remove_entities.Add(view.Name);
                    continue;
                }

                if (config == null)
                    config = (ViewElement)view;

                view.Alias = this.Convention.CreateAlias(view.Name);

                foreach (var column in view.Columns)
                {
                    column.Alias = this.Convention.CreateAlias(column.Name);

                    foreach (var constraint in column.Constraints)
                    {
                        constraint.Alias = this.Convention.CreateAlias(constraint.Name);
                    }
                }

                foreach (var constraint in view.Constraints)
                {
                    constraint.Alias = this.Convention.CreateAlias(constraint.Name);
                }
            }

            // Remove views from model that are not part of the configuration.
            foreach (var name in remove_entities)
            {
                model.Views.Remove(name);
            }
        }

        private void ApplyConfigurationToRoutines(Model model)
        {
            var routine_import_option = this.Routines.Import;
            var remove_entities = new List<string>();

            foreach (var routine in model.Routines)
            {
                var config = this.Routines.FirstOrDefault(r => r.Name.Equals(routine.Name, StringComparison.InvariantCultureIgnoreCase));

                // This view is not a part of the configuration, it should be removed from the model.
                if ((config == null 
                    && routine_import_option == ImportOption.Configured)
                    || (config != null
                    && config.Action == ImportAction.Ignore))
                {
                    remove_entities.Add(routine.Name);
                    continue;
                }

                if (config == null)
                    config = (RoutineElement)routine;

                routine.Alias = this.Convention.CreateAlias(routine.Name);

                foreach (var column in routine.Columns)
                {
                    column.Alias = this.Convention.CreateAlias(column.Name);

                    foreach (var constraint in column.Constraints)
                    {
                        constraint.Alias = this.Convention.CreateAlias(constraint.Name);
                    }
                }

                foreach (var constraint in routine.Constraints)
                {
                    constraint.Alias = this.Convention.CreateAlias(constraint.Name);
                }
            }

            // Remove routines from model that are not part of the configuration.
            foreach (var name in remove_entities)
            {
                model.Routines.Remove(name);
            }
        }

        private void ApplyConfigurationToColumns(TableElement config, Table table)
        {
            var remove_columns = new List<string>();

            foreach (var column in table.Columns)
            {
                var column_config = config.Columns.FirstOrDefault(c => c.Name.Equals(column.Name, StringComparison.InvariantCultureIgnoreCase));

                // This column is not a part of the configuration, it should be removed from the model.
                if ((column_config == null 
                    && config.Columns.Import == ImportOption.Configured)
                    || (column_config != null
                    && column_config.Action == ImportAction.Ignore))
                {
                    remove_columns.Add(column.Name);
                    continue;
                }

                if (column_config == null)
                    column_config = (ColumnElement)column;

                if (column.IsForeignKey)
                    column.Alias = this.Convention.CreateForeignKeyAlias(column);
                else
                    column.Alias = this.Convention.CreateAlias(column.Name);
            }

            // Remove columns from table that are not part of the configuration.
            foreach (var name in remove_columns)
            {
                table.Columns.Remove(name);
            }
        }

        private void ApplyConfigurationToConstraints(TableElement config, Table table)
        {
            var remove_constraints = new List<string>();

            foreach (var constraint in table.Constraints)
            {
                var constraint_config = config.Constraints.FirstOrDefault(c => c.Name.Equals(constraint.Name, StringComparison.InvariantCultureIgnoreCase));

                // This constraint is not a part of the configuration, it should be removed from the model.
                if ((constraint_config == null 
                    && config.Constraints.Import == ImportOption.Configured)
                    || (constraint_config != null
                    && constraint_config.Action == ImportAction.Ignore))
                {
                    remove_constraints.Add(constraint.Name);
                    continue;
                }

                // Check if the configuration has removed the parent table.
                // If it has, remove the constraint.
                if (constraint.GetType() == typeof(ReferentialConstraint))
                {
                    var parent_table_config = this.Tables.FirstOrDefault(t => t.Name.Equals(((ReferentialConstraint)constraint).ParentName, StringComparison.InvariantCultureIgnoreCase));

                    if ((parent_table_config == null
                        && this.Tables.Import == ImportOption.Configured)
                        || (parent_table_config != null
                        && parent_table_config.Action == ImportAction.Ignore))
                    {
                        remove_constraints.Add(constraint.Name);
                        continue;
                    }
                }

                if (constraint_config == null)
                    constraint_config = (ConstraintElement)constraint;

                if (constraint.ConstraintType == ConstraintType.ForeignKey)
                    constraint.Alias = this.Convention.CreateManyToOneAlias((ReferentialConstraint)constraint);
                else
                    constraint.Alias = this.Convention.CreateAlias(constraint.Name);

                if (constraint.GetType() == typeof(ReferentialConstraint))
                {
                    var ref_constraint = (ReferentialConstraint)constraint;
                    ref_constraint.ParentAlias = this.Convention.CreateAlias(ref_constraint.ParentName);
                }
            }

            // Remove constraints from table that are not part of the configuration.
            foreach (var name in remove_constraints)
            {
                // Find any references to the constraint in the columns.
                foreach (var column in table.Columns)
                {
                    var column_constraint = column.Constraints.FirstOrDefault(c => c.Name.Equals(name));

                    if (column_constraint != null)
                    {
                        column.Constraints.Remove(column_constraint.Name);
                    }
                }
                table.Constraints.Remove(name);
            }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Explicit conversion from a Model to a DataModelElement configuration object.
        /// </summary>
        /// <param name="obj">Model object to convert.</param>
        /// <returns>A new instance of a DataModelElement configuration object.</returns>
        public static explicit operator DataModelElement(Model obj)
        {
            return new DataModelElement(obj.Name)
            {
                Tables = (TableElementCollection)obj.Tables,
                Views = (ViewElementCollection)obj.Views,
                Routines = (RoutineElementCollection)obj.Routines
            };
        }
        #endregion

        #region Events
        #endregion
    }
}
