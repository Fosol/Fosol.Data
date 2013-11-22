using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Data.Models.Configuration
{
    public class ConstraintElement
        : ConfigurationElement
    {
        #region Variables
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

        [ConfigurationProperty("parentAlias", IsRequired = false)]
        public string ParentAlias
        {
            get { return (string)this["parentAlias"]; }
            set { this["parentAlias"] = value; }
        }

        [ConfigurationProperty("action", IsRequired = false, DefaultValue = ImportAction.Import)]
        public ImportAction Action
        {
            get { return (ImportAction)this["action"]; }
            set { this["action"] = value; }
        }
        #endregion

        #region Constructors
        public ConstraintElement()
        {

        }

        public ConstraintElement(string name, string alias = null, string parentAlias = null, ImportAction action = ImportAction.Import)
        {
            this.Name = name;
            this.Alias = alias;
            this.ParentAlias = parentAlias;
            this.Action = action;
        }
        #endregion

        #region Methods

        #endregion

        #region Events
        #endregion

        #region Operators
        public static explicit operator ConstraintElement(Constraint obj)
        {
            return new ConstraintElement(obj.Name);
        }
        #endregion
    }
}
