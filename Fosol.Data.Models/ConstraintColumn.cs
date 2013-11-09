using Fosol.Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models
{
    /// <summary>
    /// A ConstraintColumn provides a way to represent a constraint for the specified column.
    /// </summary>
    public class ConstraintColumn
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - The ordinal position of the column within this constraint.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// get - The name of the column.
        /// </summary>
        public string Name { get { return this.Column.Name; } }

        /// <summary>
        /// get - The column this constraint is attached to.
        /// </summary>
        public Column Column { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ConstraintColumn object.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter 'column' cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter 'position' must be equal to or greater than 0.</exception>
        /// <param name="position">Ordinal position of the column within the constraint.</param>
        /// <param name="column">Column object reference.</param>
        public ConstraintColumn(int position, Column column)
        {
            Assert.MinRange(position, 0, "position");
            Assert.IsNotNull(column, "column");

            this.Position = position;
            this.Column = column;
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        /// <summary>
        /// Get a unique hash code to identify this contraint column.
        /// </summary>
        /// <returns>A hash code value.</returns>
        public override int GetHashCode()
        {
            return Fosol.Common.HashCode.Create(this.Name);
        }
        #endregion

        #region Events
        #endregion
    }
}
