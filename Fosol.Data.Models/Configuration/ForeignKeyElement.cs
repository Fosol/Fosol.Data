using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fosol.Data.Models.Configuration
{
    public class ForeignKeyElement
        : ConfigurationElement
    {
        #region Variables
        #endregion

        #region Properties
        [ConfigurationProperty("pluralize", IsRequired = false, DefaultValue = true)]
        public bool Pluralize
        {
            get { return (bool)this["pluralize"]; }
            set { this["pluralize"] = value; }
        }

        [ConfigurationProperty("useColumnName", IsRequired = false, DefaultValue = true)]
        public bool UseColumnName
        {
            get { return (bool)this["useColumnName"]; }
            set { this["useColumnName"] = value; }
        }

        [ConfigurationProperty("append", IsRequired = false, DefaultValue = "Id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "AppendValue cannot be null or empty.")]
        public string AppendValue
        {
            get { return (string)this["append"]; }
            set { this["append"] = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods

        #endregion

        #region Events
        #endregion

        #region Operators
        #endregion
    }
}
