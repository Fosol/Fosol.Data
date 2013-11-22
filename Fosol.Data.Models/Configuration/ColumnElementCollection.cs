using Fosol.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Data.Models.Configuration
{
    [ConfigurationCollection(typeof(ColumnElement),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap,
        AddItemName = "add")]
    public class ColumnElementCollection
        : ConfigurationElementCollection<ColumnElement>
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
        public ColumnElementCollection()
        {

        }

        public ColumnElementCollection(ImportOption import)
        {
            this.Import = import;
        }
        #endregion

        #region Methods
        #endregion

        #region Events
        #endregion

        #region Operators
        public static explicit operator ColumnElementCollection(ColumnCollection obj)
        {
            var columns = new ColumnElementCollection();
            foreach (var column in obj)
            {
                columns.Add((ColumnElement)column);
            }
            return columns;
        }
        #endregion
    }
}
