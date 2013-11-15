using Fosol.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Data.Models.Configuration
{
    [ConfigurationCollection(typeof(ConstraintElement),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap,
        AddItemName = "add")]
    public class ConstraintElementCollection
        : ConfigurationElementCollection<ConstraintElement>
    {
        #region Variables
        #endregion

        #region Properties
        [ConfigurationProperty("import", IsRequired = false, DefaultValue = ImportOption.All)]
        public ImportOption Import
        {
            get { return (ImportOption)this.Attribute("import"); }
            set { this.Attribute("import", value); }
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
