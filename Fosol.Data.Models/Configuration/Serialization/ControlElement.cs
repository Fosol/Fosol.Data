using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "control")]
    public sealed class ControlElement
    {
        #region Variables
        #endregion

        #region Properties
        [XmlElement(ElementName = "aliases")]
        public AliasElementCollection Aliases { get; set; }

        [XmlElement(ElementName = "foreignKeys", Type = typeof(ForeignKeyElement))]
        public ForeignKeyElement ForeignKeys { get; set; }
        #endregion

        #region Constructors
        internal ControlElement()
        {
            this.Aliases = new AliasElementCollection();
            this.Aliases.Add(new AliasElement(" ", "", true));
            this.ForeignKeys = new ForeignKeyElement();
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        public static explicit operator ControlElement(Configuration.ControlElement obj)
        {
            return new ControlElement()
            {
                ForeignKeys = (ForeignKeyElement)obj.ForeignKeys,
                Aliases = (AliasElementCollection)obj.Aliases
            };
        }
        #endregion

        #region Events
        #endregion
    }
}
