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
        /// <summary>
        /// Convert the original database column type into a native .NET type.
        /// </summary>
        /// <param name="columnDbType">Original database column type.</param>
        /// <returns>A native .NET type.</returns>
        protected override Type GetNativeType(string columnDbType)
        {
            return Fosol.Common.Converters.SqlDbTypeConverter.GetNativeType(columnDbType);
        }

        /// <summary>
        /// Shallow clones the column.
        /// To perform a deep clone use the Entity class (i.e. Table, View, Routine).
        /// </summary>
        /// <returns>A new copy of this column.</returns>
        public override Column Clone()
        {
            var column = new SqlColumn(this.Name, this.SqlDbType, this.OrdinalPosition)
            {
                Alias = this.Alias,
                Default = this.Default,
                IsComputed = this.IsComputed,
                IsDeterministic = this.IsDeterministic,
                IsIdentity = this.IsIdentity,
                IsNullable = this.IsNullable,
                MaximumLength = this.MaximumLength,
                Precision = this.Precision,
                Scale = this.Scale
            };

            return column;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
