using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "constraints")]
    public sealed class ConstraintElementCollection
    {
        #region Variables
        #endregion

        #region Properties
        [XmlAttribute(AttributeName = "import")]
        public ImportOption Import { get; set; }

        [XmlElement(ElementName = "add", Type = typeof(ConstraintElement))]
        public List<ConstraintElement> Items { get; set; }
        #endregion

        #region Constructors
        internal ConstraintElementCollection()
        {
            this.Items = new List<ConstraintElement>();
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
