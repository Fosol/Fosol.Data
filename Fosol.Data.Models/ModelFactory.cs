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
        /// Generate a data Model object that represents the database specified for this ModelFactory.
        /// </summary>
        /// <returns>A new instance of a Model.</returns>
        public virtual Model Generate()
        {
            // A database has not been selected.
            if (string.IsNullOrEmpty(this.Connection.Database))
                throw new Exceptions.ModelFactoryException("The 'Connection' property must have an intial catalog (selected database).");

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
        /// Generate a default configuration for the specified 'model'.
        /// By default the model will contain 
        /// </summary>
        /// <param name="model">Model </param>
        /// <param name="convention"></param>
        /// <returns></returns>
        public Configuration.DataModelElement GenerateConfiguration(Model model, Configuration.ConventionElement convention = null)
        {
            var config = (Fosol.Data.Models.Configuration.DataModelElement)model;

            if (convention != null)
            {
                config.Convention = convention; // Not sure about this line as I added it after a long time away from this project.
                config.Alias = convention.CreateAlias(config.Name);
            }

            return config;
        }

        /// <summary>
        /// If this ModelFactory has a 'Configuration' value it will save the configuration file to the specified path.
        /// If you provide a DataModelElement for the 'model' parameter it will use it to generate the configuration file.
        /// If this ModelFactory has a 'Configuration' property value it will use it to generate the configuration file.
        /// If all else fails the ModelFactory will attempt to generate a full model from the database and use it to generate the configuration file.
        /// The default path is the assembly execution location.
        /// </summary>
        /// <param name="path">Full path and file name.</param>
        /// <param name="model">If you provide a DataModelElement object it will use it to create the configuration file.</param>
        public void SaveConfiguration(string path = "fosol.datamodel.config", Configuration.DataModelElement model = null)
        {
            var section = new Configuration.Serialization.ModelFactorySection();
            
            if (model != null)
                section.DataModels.Add((Configuration.Serialization.DataModelElement)model);
            else if (this.Configuration != null)
                section.DataModels.Add((Configuration.Serialization.DataModelElement)this.Configuration);
            else
                section.DataModels.Add((Configuration.Serialization.DataModelElement)this.GenerateConfiguration(this.Generate()));

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
