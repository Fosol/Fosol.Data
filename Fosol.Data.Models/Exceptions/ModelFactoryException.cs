using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models.Exceptions
{
    public sealed class ModelFactoryException
        : Exception
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public ModelFactoryException()
            : base()
        {
        }

        public ModelFactoryException(string message)
            : base(message)
        {

        }

        public ModelFactoryException(string message, Exception innerException)
            : base(message, innerException)
        {
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
