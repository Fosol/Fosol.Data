﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "datamodels")]
    public sealed class DataModelElementCollection
    {
        #region Variables
        #endregion

        #region Properties
        [XmlElement(ElementName = "add", Type = typeof(DataModelElement))]
        public List<DataModelElement> Items { get; set; }
        #endregion

        #region Constructors
        internal DataModelElementCollection()
        {
            this.Items = new List<DataModelElement>();
        }
        #endregion

        #region Methods
        public void Add(DataModelElement item)
        {
            this.Items.Add(item);
        }
        #endregion

        #region Operators
        public static explicit operator DataModelElementCollection(Configuration.DataModelElementCollection obj)
        {
            var models = new DataModelElementCollection();
            foreach (var model in obj)
            {
                models.Add((DataModelElement)model);
            }
            return models;
        }
        #endregion

        #region Events
        #endregion
    }
}
