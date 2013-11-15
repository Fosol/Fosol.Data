using System;
using System.Collections.Generic;
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
        [ConfigurationProperty("namespace", IsRequired = false)]
        public string Namespace
        {
            get { return (string)this["namespace"]; }
            set { this["namespace"] = value; }
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
        /// get/set - Datamodel rules to follow when building.
        /// </summary>
        [ConfigurationProperty("rules", IsRequired = false)]
        public RulesElement Rules
        {
            get { return (RulesElement)this["rules"]; }
            set { this["rules"] = value; }
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
                if (string.IsNullOrEmpty(this.ProviderName) || string.IsNullOrEmpty(this.ConnectionString))
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
            this.Tables = new TableElementCollection();
            this.Views = new ViewElementCollection();
            this.Routines = new RoutineElementCollection();
        }

        /// <summary>
        /// Creates a new instance of a DataModelElement object.
        /// </summary>
        /// <param name="name">Unique name to identify this datamodel.</param>
        public DataModelElement(string name)
            : this()
        {
            this.Name = name;
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
