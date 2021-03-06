﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fosol.Data.Models.Configuration.Serialization
{
    [XmlRoot(ElementName = "routines")]
    public sealed class RoutineElementCollection
    {
        #region Variables
        #endregion

        #region Properties
        [XmlAttribute(AttributeName = "import")]
        public ImportOption Import { get; set; }

        [XmlElement(ElementName = "add", Type = typeof(RoutineElement))]
        public List<RoutineElement> Items { get; set; }
        #endregion

        #region Constructors
        internal RoutineElementCollection()
        {
            this.Items = new List<RoutineElement>();
        }
        #endregion

        #region Methods
        public void Add(RoutineElement item)
        {
            this.Items.Add(item);
        }
        #endregion

        #region Operators
        public static explicit operator RoutineElementCollection(Configuration.RoutineElementCollection obj)
        {
            var routines = new RoutineElementCollection();
            foreach (var routine in obj)
            {
                routines.Add((RoutineElement)routine);
            }
            return routines;
        }
        #endregion

        #region Events
        #endregion
    }
}
