using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models
{
    /// <summary>
    /// The possible types of constraints.
    /// </summary>
    public enum ConstraintType
    {
        /// <summary>
        /// A primary key constraint.
        /// </summary>
        PrimaryKey = 0,
        /// <summary>
        /// A foreign key constraint.
        /// </summary>
        ForeignKey = 1,
        /// <summary>
        /// A unique key constraint.
        /// </summary>
        Unique = 2
    }
}
