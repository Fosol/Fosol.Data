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
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("alias", IsRequired = false)]
        public string Alias
        {
            get { return (string)this["alias"]; }
            set { this["alias"] = value; }
        }

        [ConfigurationProperty("namespace", IsRequired = false)]
        public string Namespace
        {
            get { return (string)this["namespace"]; }
            set { this["namespace"] = value; }
        }

        [ConfigurationProperty("providerName", IsRequired = false)]
        public string ProviderName
        {
            get { return (string)this["providerName"]; }
            set { this["providerName"] = value; }
        }

        [ConfigurationProperty("connectionString", IsRequired = false)]
        public string ConnectionString
        {
            get { return (string)this["connectionString"]; }
            set { this["connectionString"] = value; }
        }

        [ConfigurationProperty("rules", IsRequired = false)]
        public RulesElement Rules
        {
            get { return (RulesElement)this["rules"]; }
            set { this["rules"] = value; }
        }

        [ConfigurationProperty("tables", IsRequired = false)]
        public TableElementCollection Tables
        {
            get { return (TableElementCollection)this["tables"]; }
            private set { this["tables"] = value; }
        }

        [ConfigurationProperty("views", IsRequired = false)]
        public ViewElementCollection Views
        {
            get { return (ViewElementCollection)this["views"]; }
            private set { this["views"] = value; }
        }

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

                // Create a new Connection object for the specified provider.
                var factory = DbProviderFactories.GetFactory(this.ProviderName);
                _Connection = factory.CreateConnection();
                _Connection.ConnectionString = this.ConnectionString;
                return _Connection;
            }
        }
        #endregion

        #region Constructors
        public DataModelElement()
        {
            this.Tables = new TableElementCollection();
            this.Views = new ViewElementCollection();
            this.Routines = new RoutineElementCollection();
        }

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
