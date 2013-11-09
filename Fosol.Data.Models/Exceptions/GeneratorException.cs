using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models.Exceptions
{
    public sealed class GeneratorException
        : Exception
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public GeneratorException()
            : base()
        {
        }

        public GeneratorException(string message)
            : base(message)
        {

        }

        public GeneratorException(string message, Exception innerException)
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
