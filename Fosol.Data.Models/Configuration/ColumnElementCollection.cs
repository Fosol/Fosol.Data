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
