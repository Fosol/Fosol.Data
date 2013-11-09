using Fosol.Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models
{
    /// <summary>
    /// A ReferentialConstraint provides a way to identify a Constraint for a foreign key.
    /// </summary>
    public class ReferentialConstraint
        : Constraint
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - The name of the primary key constraint on the parent entity.
        /// </summary>
        public string ParentConstraintName { get; private set; }

        /// <summary>
        /// get/set - The name of the primary key table.
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// get/set - The alias name to identify the parent relationship property (used instead of the 'ParentName' property).
        /// </summary>
        public string ParentAlias { get; set; }

        /// <summary>
        /// get/set - The update rule applied to this constraint.
        /// </summary>
        public string UpdateRule { get; set; }

        /// <summary>
        /// get/set - The delete rule applied to this constraint.
        /// </summary>
        public string DeleteRule { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ReferentialConstraint object.
        /// This constructor exists so that I can late bind the ParentConstraintName property.
        /// </summary>
        /// <param name="name">A unique name to identify this referential constraint.</param>
        /// <param name="constraintType">The constraint type.</param>
        internal ReferentialConstraint(string name, ConstraintType constraintType)
            : base(name, constraintType)
        {

        }

        /// <summary>
        /// Create a new instance of a ReferentialConstraint object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameters 'name' and 'parentConstraintName' cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameters 'name' and 'parentConstraintName' cannot be null.</exception>
        /// <param name="name">A unique name to identify this referential constraint.</param>
        /// <param name="constraintType">The constraint type.</param>
        /// <param name="parentName">The name of the primary key constraint on the parent entity.</param>
        public ReferentialConstraint(string name, ConstraintType constraintType, string parentConstraintName)
            : base(name, constraintType)
        {
            Assert.IsNotNullOrEmpty(parentConstraintName, "parentConstraintName");

            this.ParentConstraintName = parentConstraintName;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set the ParentConstraintName property for this referential constraint.
        /// This method exists so that I can late bind the ParentConstraintName property.
        /// </summary>
        /// <param name="name">Name of the primary key constraint on the parent table.</param>
        internal void SetParentConstraintName(string name)
        {
            this.ParentConstraintName = name;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
