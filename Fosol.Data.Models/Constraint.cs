using Fosol.Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models
{
    /// <summary>
    /// A Constraint provides a way to represent a database constraint for a table.
    /// </summary>
    public class Constraint
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - The unique name to identify the constraint.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// get/set - A unique name to identify the constraint (used instead of the 'name' property).
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// get - The constraint type.
        /// </summary>
        public ConstraintType ConstraintType { get; private set; }

        /// <summary>
        /// get - A collection of columns associated with this constraint.
        /// </summary>
        public ConstraintColumnCollection Columns { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Constraint object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'name' cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter 'name' cannot be null.</exception>
        /// <param name="name">Unique name to identify this constraint.</param>
        /// <param name="constraintType">The type of constraint.</param>
        public Constraint(string name, ConstraintType constraintType)
        {
            Assert.IsNotNullOrEmpty(name, "name");

            this.Name = name;
            this.ConstraintType = constraintType;
            this.Columns = new ConstraintColumnCollection();
        }
        #endregion

        #region Methods

        #endregion

        #region Operators
        /// <summary>
        /// Get a unique hash code to identify this contraint.
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
