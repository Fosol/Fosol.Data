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
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Generator object.
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
        }

        /// <summary>
        /// Creates a new instance of a Generator object.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter 'connection' cannot be null.</exception>
        /// <param name="connection">DbConnection object.</param>
        public ModelFactory(DbConnection connection)
        {
            Assert.IsNotNull(connection, "connection");

            this.Connection = connection;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Builds a data Model object that represents the database specified for this Generator.
        /// </summary>
        /// <param name="config">ModelFactorySection configuration object used to control how the model will be built.</param>
        /// <returns>A new instance of a Model.</returns>
        public Model Build(Configuration.ModelFactorySection config = null)
        {
            // A database has not been selected.
            if (string.IsNullOrEmpty(this.Connection.Database))
                throw new Exceptions.GeneratorException("Before building you must select an intial catalog, or select a database within the connection.");

            var model = new Model(this.Connection.Database, config);

            try
            {
                this.Connection.Open();

                BuildTables(model);
                BuildViews(model);
                BuildRoutines(model);
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
        /// <param name="model">Model object to update with tables.</param>
        protected abstract void BuildTables(Model model);

        /// <summary>
        /// Extract the views from the database and add them to the model.
        /// </summary>
        /// <param name="model">Model object to update with views.</param>
        protected abstract void BuildViews(Model model);

        /// <summary>
        /// Extract the routines (stored procedures) from the database and add them to the model.
        /// </summary>
        /// <param name="model">Model object to update with routines.</param>
        protected abstract void BuildRoutines(Model model);

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
