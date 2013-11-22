using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "view")]
    public sealed class ViewElement
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
        internal ViewElement()
        {
            this.Columns = new ColumnElementCollection();
            this.Constraints = new ConstraintElementCollection();
        }

        internal ViewElement(string name, string alias = null, ImportAction action = ImportAction.Import)
            : this()
        {
            this.Name = name;
            this.Alias = alias;
            this.Action = action;
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        public static explicit operator ViewElement(Configuration.ViewElement obj)
        {
            return new ViewElement(obj.Name, obj.Alias, obj.Action)
            {
                Columns = (ColumnElementCollection)obj.Columns,
                Constraints = (ConstraintElementCollection)obj.Constraints
            };
        }
        #endregion

        #region Events
        #endregion
    }
}
