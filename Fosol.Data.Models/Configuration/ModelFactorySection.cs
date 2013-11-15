using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models.Configuration
{
    public class ModelFactorySection
        : ConfigurationSection
    {
        #region Variables
        public const string DefaultSectionName = "fosol.datamodel";
        #endregion

        #region Properties
        [ConfigurationProperty("xmlns", IsRequired = false)]
        public string Xmlns
        {
            get { return (string)this["xmlns"]; }
            set { this["xmlns"] = value; }
        }

        [ConfigurationProperty("datamodels", IsDefaultCollection = true, IsRequired = true)]
        public DataModelElementCollection DataModels
        {
            get { return (DataModelElementCollection)this["datamodels"]; }
            private set { this["datamodels"] = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Fetch the ModelFactorySection from the configuration with the default section name 'fosol.datamodel'.
        /// If the section has been given a custom name, this method will return null.
        /// </summary>
        /// <returns>A new instance of a ModelFactorySection object.</returns>
        public static ModelFactorySection GetDefault()
        {
            return (ModelFactorySection)System.Configuration.ConfigurationManager.GetSection(ModelFactorySection.DefaultSectionName);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
