using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Data.Models.Configuration
{
    public sealed class ViewElement
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

        [ConfigurationProperty("action", IsRequired = false, DefaultValue = ImportAction.Import)]
        public ImportAction Action
        {
            get { return (ImportAction)this["action"]; }
            set { this["action"] = value; }
        }

        [ConfigurationProperty("columns", IsRequired = false)]
        public ColumnElementCollection Columns
        {
            get { return (ColumnElementCollection)this["columns"]; }
            set { this["columns"] = value; }
        }

        [ConfigurationProperty("constraints", IsRequired = false)]
        public ConstraintElementCollection Constraints
        {
            get { return (ConstraintElementCollection)this["constraints"]; }
            set { this["constraints"] = value; }
        }
        #endregion

        #region Constructors
        public ViewElement()
        {
        }

        public ViewElement(string name, string alias = null, ImportAction action = ImportAction.Import)
        {
            this.Name = name;
            this.Alias = alias;
            this.Action = action;
        }
        #endregion

        #region Methods

        #endregion

        #region Events
        #endregion

        #region Operators
        public static explicit operator ViewElement(View obj)
        {
            return new ViewElement(obj.Name)
            {
                Columns = (ColumnElementCollection)obj.Columns,
                Constraints = (ConstraintElementCollection)obj.Constraints
            };
        }
        #endregion
    }
}
