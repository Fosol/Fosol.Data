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
        /// <summary>
        /// Add 'element' to this collection.
        /// If it's a duplicate of a prior item in the collection, the latest one will replace the prior one.
        /// </summary>
        /// <param name="element"></param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            // Replace the prior AliasElement object with the new one.
            if (this.Contains(element))
                this.Remove(((AliasElement)element).Find);
            base.BaseAdd(element);
        }
        #endregion

        #region Events
        #endregion

        #region Operators
        #endregion
    }
}
