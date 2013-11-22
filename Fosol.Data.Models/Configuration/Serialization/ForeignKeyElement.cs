using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "foreignKey")]
    public sealed class ForeignKeyElement
    {
        #region Variables
        #endregion

        #region Properties
        [XmlAttribute(AttributeName = "pluralize")]
        public bool Pluralize { get; set; }

        [XmlAttribute(AttributeName = "useColumnName")]
        public bool UseColumnName { get; set; }
        #endregion

        #region Constructors
        internal ForeignKeyElement()
        {
            this.Pluralize = true;
            this.UseColumnName = true;
        }

        internal ForeignKeyElement(bool pluralize, bool useColumnName)
        {
            this.Pluralize = pluralize;
            this.UseColumnName = UseColumnName;
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        public static explicit operator ForeignKeyElement(Configuration.ForeignKeyElement obj)
        {
            return new ForeignKeyElement(obj.Pluralize, obj.UseColumnName);
        }
        #endregion

        #region Events
        #endregion
    }
}
