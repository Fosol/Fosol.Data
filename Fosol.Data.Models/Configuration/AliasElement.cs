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

        [ConfigurationProperty("replaceWith", IsRequired = true)]
        public string ReplaceWith
        {
            get { return (string)this["replaceWith"]; }
            set { this["replaceWith"] = value; }
        }

        [ConfigurationProperty("camelCase", IsRequired = false, DefaultValue = false)]
        public bool UseCamelCase
        {
            get { return (bool)this["camelCase"]; }
            set { this["camelCase"] = value; }
        }

        [ConfigurationProperty("isRegex", IsRequired = false, DefaultValue = false)]
        public bool IsRegex
        {
            get { return (bool)this["isRegex"]; }
            set { this["isRegex"] = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Determines if the AliasElement objects are equal.  This is determined by comparing their key value 'Find'.
        /// </summary>
        /// <param name="compareTo">Object to comare to this AliasElement.</param>
        /// <returns>True if they are equal or have the same key value 'Find'.</returns>
        public override bool Equals(object compareTo)
        {
            if (compareTo.GetType() == typeof(AliasElement))
                return this.Find.Equals(((AliasElement)compareTo).Find);

            return base.Equals(compareTo);
        }

        /// <summary>
        /// Provides a unique hashcode that represents this AliasElement.
        /// </summary>
        /// <returns>Hashcode for this AliasElement.</returns>
        public override int GetHashCode()
        {
            return this.Find.GetHashCode();
        }
        #endregion

        #region Events
        #endregion

        #region Operators
        #endregion
    }
}
