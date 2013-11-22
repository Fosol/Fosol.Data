﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [Serializable]
    [XmlRoot(ElementName = "datamodel")]
    public sealed class DataModelElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get/set - A unique name to identify this datamodel.
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// get/set - An alias to use to identify this datamodel.
        /// </summary>
        [XmlAttribute(AttributeName = "alias")]
        public string Alias { get; set; }

        /// <summary>
        /// get/set - The namespace to use when creating the datamodel.
        /// </summary>
        [XmlAttribute(AttributeName = "namespace")]
        public string Namespace { get; set; }

        /// <summary>
        /// get/set - The data provider type name for the datasource.
        /// </summary>
        [XmlAttribute(AttributeName = "providerName")]
        public string ProviderName { get; set; }

        /// <summary>
        /// get/set - The connection string for the datasource.
        /// </summary>
        [XmlAttribute(AttributeName = "connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// get/set - Datamodel rules to follow when building.
        /// </summary>
        [XmlElement(ElementName = "rules")]
        public RulesElement Rules { get; set; }

        /// <summary>
        /// get/set - Collection of tables from the datasource.
        /// </summary>
        [XmlElement(ElementName = "tables")]
        public TableElementCollection Tables { get; set; }

        /// <summary>
        /// get/set - Collection of views from the datasource.
        /// </summary>
        [XmlElement(ElementName = "views")]
        public ViewElementCollection Views { get; set; }

        /// <summary>
        /// get/set - Collection of routines from the datasource.
        /// </summary>
        [XmlElement(ElementName = "routines")]
        public RoutineElementCollection Routines { get; set; }
        #endregion

        #region Constructors
        internal DataModelElement()
        {
            this.Rules = new RulesElement();
            this.Tables = new TableElementCollection();
            this.Views = new ViewElementCollection();
            this.Routines = new RoutineElementCollection();
        }

        internal DataModelElement(string name)
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
