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
        public ConstraintElementCollection()
        {

        }

        public ConstraintElementCollection(ImportOption import)
        {
            this.Import = import;
        }
        #endregion

        #region Methods
        #endregion

        #region Events
        #endregion

        #region Operators
        public static explicit operator ConstraintElementCollection(ConstraintCollection obj)
        {
            var constraints = new ConstraintElementCollection();
            foreach (var constraint in obj)
            {
                constraints.Add((ConstraintElement)constraint);
            }
            return constraints;
        }
        #endregion
    }
}
