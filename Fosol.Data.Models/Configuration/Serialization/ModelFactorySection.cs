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

        [XmlElement(ElementName = "datamodels")]
        public DataModelElementCollection DataModels { get; set; }
        #endregion

        #region Constructors
        internal ModelFactorySection()
        {
            this.DataModels = new DataModelElementCollection();
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        public static explicit operator ModelFactorySection(Configuration.ModelFactorySection obj)
        {
            return new ModelFactorySection()
            {
                DataModels = (DataModelElementCollection)obj.DataModels
            };
        }
        #endregion

        #region Events
        #endregion
    }
}
