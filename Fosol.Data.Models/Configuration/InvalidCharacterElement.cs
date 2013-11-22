using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Data.Models.Configuration
{
    public class InvalidCharacterElement
        : ConfigurationElement
    {
        #region Variables
        #endregion

        #region Properties
        [ConfigurationProperty("char", IsRequired = true, IsKey = true)]
        public string Character
        {
            get { return (string)this["char"]; }
            set { this["char"] = value; }
        }

        [ConfigurationProperty("alias", IsRequired = true)]
        public string Alias
        {
            get { return (string)this["alias"]; }
            set { this["alias"] = value; }
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
