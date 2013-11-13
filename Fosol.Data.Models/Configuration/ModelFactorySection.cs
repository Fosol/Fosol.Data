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

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
