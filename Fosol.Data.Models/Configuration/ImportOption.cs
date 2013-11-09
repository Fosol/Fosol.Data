using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models.Configuration
{
    /// <summary>
    /// Provides a way to control what entities are imported.
    /// </summary>
    public enum ImportOption
    {
        /// <summary>
        /// Do not import any of these entities.
        /// </summary>
        None = 0,
        /// <summary>
        /// Import all the entities.
        /// </summary>
        All = 1,
        /// <summary>
        /// Import only those configured.
        /// </summary>
        Configured = 2
    }
}
