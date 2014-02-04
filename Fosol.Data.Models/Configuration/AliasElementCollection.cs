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
        [ConfigurationProperty("defaultReplaceWith", IsRequired = false, DefaultValue = "")]
        public string DefaultReplaceWith
        {
            get { return (string)this.Attribute("defaultReplaceWith"); }
            set { this.Attribute("defaultReplaceWith", value); }
        }

        [ConfigurationProperty("useCamelCase", IsRequired = false, DefaultValue = true)]
        public bool UseCamelCase
        {
            get { return (bool)this.Attribute("useCamelCase"); }
            set { this.Attribute("useCamelCase", value); }
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
