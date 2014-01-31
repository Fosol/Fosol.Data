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
        /// Generates a new instance of a Model that is a copy of the original one specified.
        /// Updates the model based on the configuration settings.
        /// </summary>
        /// <param name="model">Model object to clone and apply configuration settings to.</param>
        /// <returns>A new instance of a Model with the configuration settings applied to it.</returns>
        public Model ApplyConfiguration(Model model)
        {
            var clone = model.Clone();

            clone.Alias = this.Convention.CreateAlias(model.Name);

            foreach (var table in clone.Tables)
            {
                table.Alias = this.Convention.CreateAlias(table.Name);

                foreach (var column in table.Columns)
                {
                    column.Alias = this.Convention.CreateAlias(column.Name);

                    foreach (var constraint in column.Constraints)
                    {
                        constraint.Alias = this.Convention.CreateAlias(constraint.Name);
                    }
                }

                foreach (var constraint in table.Constraints)
                {
                    constraint.Alias = this.Convention.CreateAlias(constraint.Name);
                }
            }

            foreach (var view in clone.Views)
            {
                view.Alias = this.Convention.CreateAlias(view.Name);
            }

            foreach (var routine in clone.Routines)
            {
                routine.Alias = this.Convention.CreateAlias(routine.Name);
            }

            return clone;
        }
        #endregion

        #region Operators
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
