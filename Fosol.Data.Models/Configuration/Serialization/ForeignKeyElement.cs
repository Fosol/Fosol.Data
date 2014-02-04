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

        [XmlAttribute(AttributeName = "append")]
        public string AppendValue { get; set; }
        #endregion

        #region Constructors
        internal ForeignKeyElement()
        {
            this.Pluralize = true;
            this.UseColumnName = true;
            this.AppendValue = "Id";
        }

        internal ForeignKeyElement(bool pluralize, bool useColumnName, string appendValue)
        {
            this.Pluralize = pluralize;
            this.UseColumnName = UseColumnName;
            this.AppendValue = appendValue;
        }
        #endregion

        #region Methods
        #endregion

        #region Operators
        /// <summary>
        /// Explicit conversion from a Configuration.ForeignKeyElement to a Serialization.ForeignKeyElement.
        /// </summary>
        /// <param name="obj">Configuration.ForeignKeyElement object.</param>
        /// <returns>A new instance of a Serialization.ForeignKeyElement object.</returns>
        public static explicit operator ForeignKeyElement(Configuration.ForeignKeyElement obj)
        {
            return new ForeignKeyElement(obj.Pluralize, obj.UseColumnName, obj.AppendValue);
        }
        #endregion

        #region Events
        #endregion
    }
}
