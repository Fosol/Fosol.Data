using Fosol.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models.Configuration
{
    [ConfigurationCollection(typeof(DataModelElement),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap,
        AddItemName = "add")]
    public class DataModelElementCollection
        : ConfigurationElementCollection<DataModelElement>
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
