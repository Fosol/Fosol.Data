using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "table")]
    public sealed class TableElement
    {
        #region Variables
        #endregion

        #region Properties
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "alias")]
        public string Alias { get; set; }

        [XmlAttribute(AttributeName = "action")]
        public ImportAction Action { get; set; }

        public ColumnElementCollection Columns { get; set; }

        public ConstraintElementCollection Constraints { get; set; }
        #endregion

        #region Constructors
        internal TableElement()
        {
            this.Columns = new ColumnElementCollection();
            this.Constraints = new ConstraintElementCollection();
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
