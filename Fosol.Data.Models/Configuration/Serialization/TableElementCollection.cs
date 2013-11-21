﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "tables")]
    public sealed class TableElementCollection
    {
        #region Variables
        #endregion

        #region Properties
        [XmlAttribute(AttributeName = "import")]
        public ImportOption Import { get; set; }

        [XmlElement(ElementName = "add", Type = typeof(TableElement))]
        public List<TableElement> Items { get; set; }
        #endregion

        #region Constructors
        internal TableElementCollection()
        {
            this.Items = new List<TableElement>();
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