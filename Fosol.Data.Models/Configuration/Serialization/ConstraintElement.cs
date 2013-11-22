using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "constraint")]
    public sealed class ConstraintElement
    {
        #region Variables
        #endregion

        #region Properties
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "alias")]
        public string Alias { get; set; }

        [XmlAttribute(AttributeName = "parentAlias")]
        public string ParentAlias { get; set; }

        [XmlAttribute(AttributeName = "action")]
        public ImportAction Action { get; set; }
        #endregion

        #region Constructors
        internal ConstraintElement()
        {
            this.Action = ImportAction.Import;
        }

        internal ConstraintElement(string name)
            : this()
        {
            this.Name = name;
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
