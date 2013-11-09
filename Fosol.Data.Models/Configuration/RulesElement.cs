using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Data.Models.Configuration
{
    public sealed class RulesElement
        : ConfigurationElement
    {
        #region Variables
        #endregion

        #region Properties
        [ConfigurationProperty("invalidCharacters", IsRequired = false)]
        public InvalidCharacterElementCollection InvalidCharacters
        {
            get { return (InvalidCharacterElementCollection)this["invalidCharacters"]; }
            set { this["invalidCharacters"] = value; }
        }

        [ConfigurationProperty("foreignKeys", IsRequired = false)]
        public ForeignKeyElement ForeignKeys
        {
            get { return (ForeignKeyElement)this["foreignKeys"]; }
            set { this["foreignKeys"] = value; }
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
