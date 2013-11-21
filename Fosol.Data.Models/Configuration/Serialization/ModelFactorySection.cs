using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [Serializable]
    [XmlRoot(ElementName = "fosol.datamodel", Namespace = "http://schemas.fosol.ca/DataModel.xsd")]
    public sealed class ModelFactorySection
    {
        #region Variables
        #endregion

        #region Properties

        [XmlElement(ElementName = "add", Type = typeof(DataModelElement))]
        public List<DataModelElement> DataModels { get; set; }
        #endregion

        #region Constructors
        internal ModelFactorySection()
        {
            this.DataModels = new List<DataModelElement>();
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
