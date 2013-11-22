using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "column")]
    public sealed class ColumnElement
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
        #endregion

        #region Constructors
        internal ColumnElement()
        {
            this.Action = ImportAction.Import;
        }

        internal ColumnElement(string name, string alias = null, ImportAction action = ImportAction.Import)
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
        public static explicit operator ColumnElement(Configuration.ColumnElement obj)
        {
            return new ColumnElement(obj.Name, obj.Alias, obj.Action);
        }
        #endregion

        #region Events
        #endregion
    }
}
