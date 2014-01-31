using Fosol.Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models
{
    /// <summary>
    /// An Entity object represents a table or view from a database.
    /// An Entity contains information to recreate a table or view, its columns and constraints.
    /// </summary>
    public abstract class Entity
        : ICloneable
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - A unique name to identify this entity.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// get/set - An alias to use instead of the oringal name.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// get - The type of entity this object represents from the database.
        /// </summary>
        public string EntityType { get; private set; }

        /// <summary>
        /// get - The database schema.
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// get - The catalog this entity belongs to.
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// get - A ColumnCollection containing the columns for this entity.
        /// </summary>
        public ColumnCollection Columns { get; private set; }

        /// <summary>
        /// get - A ConstrainCollection containing all constraints for this entity.
        /// </summary>
        public ConstraintCollection Constraints { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an Entity object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameters 'name' and 'entityType' cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameters 'name' and 'entityType' cannot be null.</exception>
        /// <param name="name">A unique name to identify this entity.</param>
        /// <param name="entityType">The type this entity represents from the database.</param>
        public Entity(string name, string entityType)
        {
            Assert.IsNotNullOrEmpty(name, "name");
            Assert.IsNotNullOrEmpty(entityType, "entityType");

            this.Name = name;
            this.EntityType = entityType;
            this.Columns = new ColumnCollection();
            this.Constraints = new ConstraintCollection();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determine if this entity contains a column with the specified name.
        /// </summary>
        /// <param name="name">Name to identify the column.</param>
        /// <returns>True if the column exists.</returns>
        public bool ContainsColumn(string name)
        {
            return this.Columns.ContainsColumn(name);
        }

        /// <summary>
        /// Determine if this entity contains a constraint with the specified name.
        /// </summary>
        /// <param name="name">Name to identify the constraint.</param>
        /// <returns>True if the constraint exists.</returns>
        public bool ContainsConstraint(string name)
        {
            return this.Constraints.ContainsConstraint(name);
        }
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

        /// <summary>
        /// Deep clone this object.
        /// </summary>
        /// <returns>A new copy of this object.</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Deep clone this object.
        /// </summary>
        /// <returns>A new copy of this object.</returns>
        public abstract Entity Clone();
        #endregion

        #region Events
        #endregion
    }
}
