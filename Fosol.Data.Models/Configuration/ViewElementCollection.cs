using Fosol.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Data.Models.Configuration
{
    [ConfigurationCollection(typeof(ViewElement),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap,
        AddItemName = "add")]
    public class ViewElementCollection
        : ConfigurationElementCollection<ViewElement>
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
        public static explicit operator ViewElementCollection(EntityCollection<View> obj)
        {
            var views = new ViewElementCollection();
            foreach (var view in obj)
            {
                views.Add((ViewElement)view);
            }
            return views;
        }
        #endregion
    }
}
