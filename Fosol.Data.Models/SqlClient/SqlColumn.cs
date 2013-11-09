using Fosol.Common.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models.SqlClient
{
    /// <summary>
    /// A SqlColumn provides a way to define a single column within an Sql Server data structure.
    /// </summary>
    public sealed class SqlColumn
        : Column
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - The SqlDbType of the column.
        /// </summary>
        public SqlDbType SqlDbType { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a SqlColumn object.
        /// </summary>
        /// <param name="name">The unique name of the column.</param>
        /// <param name="sqlDbType">The SqlDbType of the column.</param>
        /// <param name="ordinalPosition">The unique ordinal position of the column within the table.</param>
        public SqlColumn(string name, SqlDbType sqlDbType, int ordinalPosition)
            : base(name, sqlDbType.ToString("g"), ordinalPosition)
        {
            this.SqlDbType = sqlDbType;
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
