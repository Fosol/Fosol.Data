using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "invalidCharacters")]
    public sealed class InvalidCharacterElementCollection
    {
        #region Variables
        #endregion

        #region Properties
        [XmlAttribute(AttributeName = "defaultAlias")]
        public string DefaultAlias { get; set; }

        [XmlElement(ElementName = "add", Type = typeof(InvalidCharacterElement))]
        public List<InvalidCharacterElement> Items { get; set; }

        public InvalidCharacterElement this[int index]
        {
            get { return this.Items[index]; }
            set { this.Items[index] = value; }
        }
        #endregion

        #region Constructors
        internal InvalidCharacterElementCollection()
        {
            this.Items = new List<InvalidCharacterElement>();
            this.DefaultAlias = "_";
        }
        #endregion

        #region Methods
        public void Add(InvalidCharacterElement item)
        {
            this.Items.Add(item);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
