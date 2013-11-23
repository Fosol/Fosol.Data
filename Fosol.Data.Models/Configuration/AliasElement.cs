using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Data.Models.Configuration
{
    public class AliasElement
        : ConfigurationElement
    {
        #region Variables
        #endregion

        #region Properties
        [ConfigurationProperty("find", IsRequired = true, IsKey = true)]
        public string Find
        {
            get { return (string)this["find"]; }
            set { this["find"] = value; }
        }

        [ConfigurationProperty("replace", IsRequired = true)]
        public string Replace
        {
            get { return (string)this["replace"]; }
            set { this["replace"] = value; }
        }

        [ConfigurationProperty("camelCase", IsRequired = false, DefaultValue = false)]
        public bool UseCamelCase
        {
            get { return (bool)this["camelCase"]; }
            set { this["camelCase"] = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods

        #endregion

        #region Events
        #endregion

        #region Operators
        #endregion
    }
}
