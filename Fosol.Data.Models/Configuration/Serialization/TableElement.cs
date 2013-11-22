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

        [XmlElement(ElementName = "columns")]
        public ColumnElementCollection Columns { get; set; }

        [XmlElement(ElementName = "constraints")]
        public ConstraintElementCollection Constraints { get; set; }
        #endregion

        #region Constructors
        internal TableElement()
        {
            this.Action = ImportAction.Import;
            this.Columns = new ColumnElementCollection();
            this.Constraints = new ConstraintElementCollection();
        }

        internal TableElement(string name)
            : this()
        {
            this.Name = name;
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
