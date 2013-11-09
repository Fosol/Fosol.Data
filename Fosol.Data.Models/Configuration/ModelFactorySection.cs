using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models.Configuration
{
    public class ModelFactorySection
        : ConfigurationSection
    {
        #region Variables
        #endregion

        #region Properties
        [ConfigurationProperty("xmlns", IsRequired = false)]
        public string Xmlns
        {
            get { return (string)this["xmlns"]; }
            set { this["xmlns"] = value; }
        }

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
        #endregion

        #region Constructors
        public ModelFactorySection()
        {
            this.Tables = new TableElementCollection();
            this.Views = new ViewElementCollection();
            this.Routines = new RoutineElementCollection();
        }

        public ModelFactorySection(string name)
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
