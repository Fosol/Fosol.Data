using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "columns")]
    public sealed class ColumnElementCollection
    {
        #region Variables
        #endregion

        #region Properties
        [XmlAttribute(AttributeName = "import")]
        public ImportOption Import { get; set; }

        [XmlElement(ElementName = "add", Type = typeof(ColumnElement))]
        public List<ColumnElement> Items { get; set; }
        #endregion

        #region Constructors
        internal ColumnElementCollection()
        {
            this.Items = new List<ColumnElement>();
        }
        #endregion

        #region Methods
        public void Add(ColumnElement item)
        {
            this.Items.Add(item);
        }
        #endregion

        #region Operators
        public static explicit operator ColumnElementCollection(Configuration.ColumnElementCollection obj)
        {
            var columns = new ColumnElementCollection();
            foreach (var column in obj)
            {
                columns.Add((ColumnElement)column);
            }
            return columns;
        }
        #endregion

        #region Events
        #endregion
    }
}
