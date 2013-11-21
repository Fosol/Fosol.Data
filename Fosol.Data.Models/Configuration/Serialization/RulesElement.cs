using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "rules")]
    public sealed class RulesElement
    {
        #region Variables
        #endregion

        #region Properties
        [XmlElement(ElementName = "invalidCharacters")]
        public InvalidCharacterElementCollection InvalidCharacters { get; set; }

        [XmlElement(ElementName = "foreignKeys", Type = typeof(ForeignKeyElement))]
        public ForeignKeyElement ForeignKeys { get; set; }
        #endregion

        #region Constructors
        internal RulesElement()
        {
            this.InvalidCharacters = new InvalidCharacterElementCollection();
            this.InvalidCharacters.Add(new InvalidCharacterElement(" ", "", true));
            this.ForeignKeys = new ForeignKeyElement();
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
