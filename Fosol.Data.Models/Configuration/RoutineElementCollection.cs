using Fosol.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Data.Models.Configuration
{
    [ConfigurationCollection(typeof(RoutineElement),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap,
        AddItemName = "add")]
    public class RoutineElementCollection
        : ConfigurationElementCollection<RoutineElement>
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
        public static explicit operator RoutineElementCollection(EntityCollection<Routine> obj)
        {
            var routines = new RoutineElementCollection();
            foreach (var routine in obj)
            {
                routines.Add((RoutineElement)routine);
            }
            return routines;
        }
        #endregion
    }
}
