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

        [XmlAttribute(AttributeName = "isRegex")]
        public bool IsRegex { get; set; }
        #endregion

        #region Constructors
        internal AliasElement()
        {
        }

        internal AliasElement(string find, string replace = "", bool useCamelCase = true, bool isRegex = false)
        {
            this.Find = find;
            this.Replace = replace;
            this.UseCamelCase = useCamelCase;
            this.IsRegex = IsRegex;
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        public static explicit operator AliasElement(Configuration.AliasElement obj)
        {
            return new AliasElement(obj.Find, obj.ReplaceWith, obj.UseCamelCase, obj.IsRegex);
        }
        #endregion

        #region Events
        #endregion
    }
}
