using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "invalidCharacter")]
    public sealed class AliasElement
    {
        #region Variables
        #endregion

        #region Properties
        [XmlAttribute(AttributeName = "find")]
        public string Find { get; set; }

        [XmlAttribute(AttributeName = "replace")]
        public string Replace { get; set; }

        [XmlAttribute(AttributeName = "camelCase")]
        public bool UseCamelCase { get; set; }
        #endregion

        #region Constructors
        internal AliasElement()
        {
        }

        internal AliasElement(string find, string replace = "", bool useCamelCase = true)
        {
            this.Find = find;
            this.Replace = replace;
            this.UseCamelCase = useCamelCase;
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        public static explicit operator AliasElement(Configuration.AliasElement obj)
        {
            return new AliasElement(obj.Find, obj.Replace, obj.UseCamelCase);
        }
        #endregion

        #region Events
        #endregion
    }
}
