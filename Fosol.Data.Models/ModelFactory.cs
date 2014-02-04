using Fosol.Common.Validation;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Fosol.Data.Models
{
    /// <summary>
    /// A ModelFactory provides a way to automate the process of generating a Model from a database.
    /// ModelFactory is an abstract class for specific data provider model factories (i.e. SqlModelFactory).
    /// </summary>
    public abstract class ModelFactory
    {
        #region Variabless
        #endregion

        #region Properties
        /// <summary>
        /// get - The DbConnection object used to connect to the database.
        /// </summary>
        protected DbConnection Connection { get; set; }

        /// <summary>
        /// get - The DataModelElement configuration object.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Property 'Configuration' cannot be null.</exception>
        public Configuration.DataModelElement Configuration { get; private set; }
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

            // Create a default configuration.
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

            // Create a default configuration.
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
        /// Generate a data Model object that represents the database specified in the configuration.
        /// Apply the configuration to the model so that the model will be modified.
        /// </summary>
        /// <returns>A new instance of a Model.</returns>
        public Model Generate()
        {
            var model = OnGenerate();

            // Apply configuration to the model.
            this.Configuration.ApplyConfiguration(model);

            return model;
        }

        /// <summary>
        /// Generate a data Model object that represents the database specified in the configuration.
        /// </summary>
        /// <returns>A new instance of a Model.</returns>
        protected virtual Model OnGenerate()
        {
            var model = new Model(this.Connection.Database);

            try
            {
                this.Connection.Open();

                model.Tables.Merge(GetTables());
                model.Views.Merge(GetViews());
                model.Routines.Merge(GetRoutines());
            }
            finally
            {
                this.Connection.Close();
            }

            return model;
        }

        /// <summary>
        /// Generate a DataModelElement configuration object for the data Model this ModelFactory generates.
        /// Does not apply any configuration options to the model.
        /// </summary>
        /// <returns></returns>
        public Configuration.DataModelElement GenerateConfiguration()
        {
            return ModelFactory.GenerateConfiguration(this.OnGenerate());
        }

        /// <summary>
        /// Generate a configuration for the 'model' specified.
        /// </summary>
        /// <param name="model">Model object you want to create a configuration for.</param>
        /// <exception cref="System.ArgumentNullException">Paramter 'model' cannot be null.</exception>
        /// <returns>DataModelElement object for the Model generated by this ModelFactory.</returns>
        public static Configuration.DataModelElement GenerateConfiguration(Model model)
        {
            Fosol.Common.Validation.Assert.IsNotNull(model, "model");
            return (Fosol.Data.Models.Configuration.DataModelElement)model;
        }

        /// <summary>
        /// Save the configuration to a file.
        /// The default path is the assembly execution location.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'path' cannot be empty.</exception>
        /// <param name="path">Full path and file name.</param>
        public void SaveConfiguration(string path = "fosol.datamodel.config")
        {
            ModelFactory.SaveConfiguration(this.Configuration, path);
        }

        /// <summary>
        /// Save the configuration to a file.
        /// The default path is the assembly execution location.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'path' cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameters 'config' and 'path' cannot be null.</exception>
        /// <param name="config">DataModelElement configuration object to save as a file.</param>
        /// <param name="path">Full path and file name.</param>
        public static void SaveConfiguration(Configuration.DataModelElement config, string path = "fosol.datamodel.config")
        {
            Fosol.Common.Validation.Assert.IsNotNull(config, "config");
            Fosol.Common.Validation.Assert.IsNotNullOrEmpty(path, "path");
            var section = new Configuration.Serialization.ModelFactorySection();
            section.DataModels.Add((Configuration.Serialization.DataModelElement)config);
            Fosol.Common.Serialization.XmlHelper.SerializeToFile(section, path, System.IO.FileMode.Create);
        }

        /// <summary>
        /// Extract the tables from the database and add them to the model.
        /// </summary>
        protected abstract EntityCollection<Table> GetTables();

        /// <summary>
        /// Extract the views from the database and add them to the model.
        /// </summary>
        protected abstract EntityCollection<View> GetViews();

        /// <summary>
        /// Extract the routines (stored procedures) from the database and add them to the model.
        /// </summary>
        protected abstract EntityCollection<Routine> GetRoutines();

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
