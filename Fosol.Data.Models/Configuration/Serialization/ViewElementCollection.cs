﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "views")]
    public sealed class ViewElementCollection
    {
        #region Variables
        #endregion

        #region Properties
        [XmlAttribute(AttributeName = "import")]
        public ImportOption Import { get; set; }

        [XmlElement(ElementName = "add", Type = typeof(ViewElement))]
        public List<ViewElement> Items { get; set; }
        #endregion

        #region Constructors
        internal ViewElementCollection()
        {
            this.Items = new List<ViewElement>();
        }
        #endregion

        #region Methods
        public void Add(ViewElement item)
        {
            this.Items.Add(item);
        }
        #endregion

        #region Operators
        public static explicit operator ViewElementCollection(Configuration.ViewElementCollection obj)
        {
            var views = new ViewElementCollection();
            foreach (var view in obj)
            {
                views.Add((ViewElement)view);
            }
            return views;
        }
        #endregion

        #region Events
        #endregion
    }
}
