using Fosol.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Data.Models.Configuration
{
    [ConfigurationCollection(typeof(AliasElement),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap,
        AddItemName = "add")]
    public class AliasElementCollection
        : ConfigurationElementCollection<AliasElement>
    {
        #region Variables
        #endregion

        #region Properties
        [ConfigurationProperty("default", IsRequired = false, DefaultValue = "_")]
        public string Default
        {
            get { return (string)this.Attribute("default"); }
            set { this.Attribute("default", value); }
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
