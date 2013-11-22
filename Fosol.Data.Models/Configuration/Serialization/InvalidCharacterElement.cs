using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "invalidCharacter")]
    public sealed class InvalidCharacterElement
    {
        #region Variables
        #endregion

        #region Properties
        [XmlAttribute(AttributeName = "char")]
        public string Char { get; set; }

        [XmlAttribute(AttributeName = "alias")]
        public string Alias { get; set; }

        [XmlAttribute(AttributeName = "camelCase")]
        public bool UseCamelCase { get; set; }
        #endregion

        #region Constructors
        internal InvalidCharacterElement()
        {
        }

        internal InvalidCharacterElement(string invalidChar, string alias = "", bool useCamelCase = true)
        {
            this.Char = invalidChar;
            this.Alias = alias;
            this.UseCamelCase = useCamelCase;
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
