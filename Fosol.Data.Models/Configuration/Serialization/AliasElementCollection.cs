using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "aliases")]
    public sealed class AliasElementCollection
    {
        #region Variables
        #endregion

        #region Properties
        [XmlAttribute(AttributeName = "defaultReplaceWith")]
        public string DefaultReplaceWith { get; set; }

        [XmlAttribute(AttributeName = "useCamelCase")]
        public bool UseCamelCase { get; set; }

        [XmlElement(ElementName = "add", Type = typeof(AliasElement))]
        public List<AliasElement> Items { get; set; }

        public AliasElement this[int index]
        {
            get { return this.Items[index]; }
            set { this.Items[index] = value; }
        }
        #endregion

        #region Constructors
        internal AliasElementCollection()
        {
            this.Items = new List<AliasElement>();
            this.DefaultReplaceWith = "_";
            this.UseCamelCase = true;
        }
        #endregion

        #region Methods
        public void Add(AliasElement item)
        {
            this.Items.Add(item);
        }
        #endregion

        #region Operators
        public static explicit operator AliasElementCollection(Configuration.AliasElementCollection obj)
        {
            var aliases = new AliasElementCollection();
            aliases.UseCamelCase = obj.UseCamelCase;
            aliases.DefaultReplaceWith = obj.DefaultReplaceWith;
            foreach (var alias in obj)
            {
                aliases.Add((AliasElement)alias);
            }
            return aliases;
        }
        #endregion

        #region Events
        #endregion
    }
}
