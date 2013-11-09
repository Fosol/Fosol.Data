using Fosol.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Data.Models.Configuration
{
    [ConfigurationCollection(typeof(InvalidCharacterElement),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap,
        AddItemName = "add")]
    public class InvalidCharacterElementCollection
        : ConfigurationElementCollection<InvalidCharacterElement>
    {
        #region Variables
        #endregion

        #region Properties
        [ConfigurationProperty("defaultAlias", IsRequired = false, DefaultValue = "_")]
        public string DefaultAlias
        {
            get { return (string)this.Attribute("defaultAlias"); }
            set { this.Attribute("defaultAlias", value); }
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
