using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models.Configuration
{
    /// <summary>
    /// Provides a way to control how an entity is imported.
    /// </summary>
    public enum ImportAction
    {
        /// <summary>
        /// Do not import this entity.
        /// </summary>
        Ignore = 0,
        /// <summary>
        /// Import this entity.
        /// </summary>
        Import = 1
    }
}
