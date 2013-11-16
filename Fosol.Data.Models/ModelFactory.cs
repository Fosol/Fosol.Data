using Fosol.Common.Validation;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models
{
    /// <summary>
    /// A ModelFactory provides a way to automate the process of generating a Model from on a database.
    /// ModelFactory is an abstract class for specific data provider model factories (i.e. SqlModelFactory).
    /// </summary>
    public abstract class ModelFactory
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - The DbConnection object used to connect to the database.
        /// </summary>
        public DbConnection Connection { get; protected set; }

        /// <summary>
        /// get - The DataModelElement configuration object.
        /// </summary>
        public Configuration.DataModelElement Configuration { get; protected set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ModelFactory object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameters 'providerInvariantName' and 'connectionString' cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameters 'providerInvariantName' and 'connectionString' cannot be null.</exception>
        /// <param name="providerInvariantName">A valid provider name.</param>
        /// <param name="connectionString">A connection string or the name in the configuration file.</param>
        protected ModelFactory(string providerInvariantName, string connectionString)
        {
            Assert.IsNotNullOrEmpty(providerInvariantName, "providerInvariantName");
            Assert.IsNotNullOrEmpty(connectionString, "connectionString");

            var cs = System.Configuration.ConfigurationManager.ConnectionStrings[connectionString];
            if (cs == null)
            {
                // Figure out which DbConnection type to use based on the provider.
                var build = new DbConnectionStringBuilder();
                build.ConnectionString = connectionString;
                var factory = DbProviderFactories.GetFactory(providerInvariantName);

                this.Connection = factory.CreateConnection();
                this.Connection.ConnectionString = build.ConnectionString;
            }
            else
            {
                var factory = DbProviderFactories.GetFactory(cs.ProviderName);

                this.Connection = factory.CreateConnection();
                this.Connection.ConnectionString = cs.ConnectionString;
            }

            if (string.IsNullOrEmpty(this.Connection.Database))
                throw new Exceptions.ModelFactoryException("The 'Connection' property must have an intial catalog (selected database).");

            this.Configuration = new Configuration.DataModelElement(this.Connection.Database);
        }

        /// <summary>
        /// Creates a new instance of a ModelFactory object.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter 'connection' cannot be null.</exception>
        /// <param name="connection">DbConnection object.</param>
        public ModelFactory(DbConnection connection)
        {
            Assert.IsNotNull(connection, "connection");

            this.Connection = connection;

            if (string.IsNullOrEmpty(this.Connection.Database))
                throw new Exceptions.ModelFactoryException("The 'Connection' property must have an intial catalog (selected database).");

            this.Configuration = new Configuration.DataModelElement(this.Connection.Database);
        }

        /// <summary>
        /// Creates a new instance of a ModelFactory object.
        /// </summary>
        /// <param name="config">DataModelElement configuration object.</param>
        public ModelFactory(Configuration.DataModelElement config)
        {
            Assert.IsNotNull(config, "config");
            Assert.IsNotNull(config.Connection, "config.Connection", "The 'Connection' property cannot be null.");

            this.Configuration = config;
            this.Connection = config.Connection;

            if (string.IsNullOrEmpty(this.Connection.Database))
                throw new Exceptions.ModelFactoryException("The 'Connection' property must have an intial catalog (selected database).");
        }

        /// <summary>
        /// Creates a new instance of a ModelFactory object.
        /// This constructor will only work if there is a ModelFactorySection configuration within your application and it contains a datamodel with the specified name.
        /// </summary>
        /// <param name="modelName">The name to identify the datamodel within the configuration section.</param>
        public ModelFactory(string modelName)
        {
            Assert.IsNotNullOrEmpty(modelName, "modelName");

            var config = Fosol.Data.Models.Configuration.ModelFactorySection.GetDefault();

            if (config == null)
                throw new Exceptions.ModelFactoryException(string.Format("Configuration section '{0}' missing.", Fosol.Data.Models.Configuration.ModelFactorySection.DefaultSectionName));

            this.Configuration = config.DataModels[modelName];

            if (this.Configuration == null)
                throw new Exceptions.ModelFactoryException(string.Format("Configuration section does not contain a datamodel named '{0}'.", modelName));

            this.Connection = this.Configuration.Connection;

            if (string.IsNullOrEmpty(this.Connection.Database))
                throw new Exceptions.ModelFactoryException("The 'Connection' property must have an intial catalog (selected database).");
        }
        #endregion

        #region Methods
        /// <summary>
        /// Builds a data Model object that represents the database specified for this ModelFactory.
        /// </summary>
        /// <returns>A new instance of a Model.</returns>
        public Model Build()
        {
            // A database has not been selected.
            if (string.IsNullOrEmpty(this.Connection.Database))
                throw new Exceptions.ModelFactoryException("The 'Connection' property must have an intial catalog (selected database).");

            var model = new Model(this.Connection.Database);

            try
            {
                this.Connection.Open();

                // Based on the configuration import the appropriate tables into the model.
                var config_tables = this.Configuration.Tables;
                if (config_tables.Import == Models.Configuration.ImportOption.All || config_tables.Count > 0)
                {
                    // Fetch the tables from the datasource.
                    foreach (var table in BuildTables())
                    {
                        var table_config = config_tables.FirstOrDefault(t => t.Name.Equals(table.Name, StringComparison.InvariantCultureIgnoreCase));

                        // Do not include tables that have not been configured, or have been configued to be ignored.
                        if (config_tables.Import == Models.Configuration.ImportOption.Configured && (table_config == null || table_config.Action == Models.Configuration.ImportAction.Ignore))
                            continue;
                        else if (config_tables.Import == Models.Configuration.ImportOption.All && table_config == null)
                            model.Entities.Add(table);
                        else if (table_config != null)
                        {
                            // Update the table with the configuration properties.
                            table.Alias = table_config.Alias;

                            // Only import the appropriate constraints.
                            if (table_config.Constraints.Import == Models.Configuration.ImportOption.All || table_config.Constraints.Count > 0)
                            {
                                var names = table.Constraints.GetNames();
                                foreach (var name in names)
                                {
                                    var constraint = table.Constraints[name];
                                    var constraint_config = table_config.Constraints.FirstOrDefault(c => c.Name.Equals(constraint.Name, StringComparison.InvariantCultureIgnoreCase));

                                    if (constraint_config == null && table_config.Constraints.Import == Models.Configuration.ImportOption.Configured)
                                        table.Constraints.Remove(constraint);
                                    else if (constraint_config != null)
                                    {
                                        if (constraint_config.Action == Models.Configuration.ImportAction.Import)
                                        {
                                            // Update the constraint with the configuration properties.
                                            constraint.Alias = constraint_config.Alias;
                                        }
                                        else
                                        {
                                            // This constraint was configured to be ignored.
                                            table.Constraints.Remove(constraint);
                                        }
                                    }
                                }
                            }

                            // Only import the appropriate columns.
                            if (table_config.Columns.Import == Models.Configuration.ImportOption.All || table_config.Columns.Count > 0)
                            {
                                var names = table.Columns.GetNames();
                                foreach (var name in names)
                                {
                                    var column = table.Columns[name];
                                    var column_config = table_config.Columns.FirstOrDefault(c => c.Name.Equals(column.Name, StringComparison.InvariantCultureIgnoreCase));

                                    if (column_config == null && table_config.Columns.Import == Models.Configuration.ImportOption.Configured)
                                        table.Columns.Remove(column);
                                    else if (column_config != null)
                                    {
                                        if (column_config.Action == Models.Configuration.ImportAction.Import)
                                        {
                                            // Update the column with the configuration properties.
                                            column.Alias = column_config.Alias;
                                        }
                                        else
                                        {
                                            // This column was configured to be ignored.
                                            table.Columns.Remove(column);
                                        }
                                    }
                                }
                            }

                            model.Entities.Add(table);
                        }
                    }
                }
                //model.Entities.Merge(BuildViews());
                //model.Entities.Merge(BuildRoutines());
            }
            finally
            {
                this.Connection.Close();
            }

            return model;
        }

        /// <summary>
        /// Extract the tables from the database and add them to the model.
        /// </summary>
        protected abstract EntityCollection BuildTables();

        /// <summary>
        /// Extract the views from the database and add them to the model.
        /// </summary>
        protected abstract EntityCollection BuildViews();

        /// <summary>
        /// Extract the routines (stored procedures) from the database and add them to the model.
        /// </summary>
        protected abstract EntityCollection BuildRoutines();

        /// <summary>
        /// Extract the constraint type and return an enum value.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The 'value' parameter is an invalid constraint type.</exception>
        /// <param name="value">A constraint type.</param>
        /// <returns>ConstraintType enum value.</returns>
        protected static ConstraintType ParseConstraintType(string value)
        {
            switch (value)
            {
                case ("FOREIGN KEY"):
                    return ConstraintType.ForeignKey;
                case ("PRIMARY KEY"):
                    return ConstraintType.PrimaryKey;
                case ("UNIQUE"):
                    return ConstraintType.Unique;
            }

            throw new InvalidOperationException("ConstraintType does not exist.");
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
