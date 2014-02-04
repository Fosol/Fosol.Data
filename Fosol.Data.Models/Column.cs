using Fosol.Common.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models
{
    /// <summary>
    /// A Column object provides a representation of a database column.
    /// You will need to create a data provider specific Column (i.e. SqlColumn).
    /// </summary>
    public abstract class Column
        : ICloneable
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - A unique name to identify this column.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// get/set - An alias to use instead of the oringal name.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// get - The original database type of the column.
        /// </summary>
        public string DbType { get; private set; }

        /// <summary>
        /// get - The native .NET type for this column.
        /// </summary>
        public Type NativeType { get; private set; }

        /// <summary>
        /// get - The ordinal position this column is in within the table.
        /// </summary>
        public int OrdinalPosition { get; private set; }

        /// <summary>
        /// get/set - Whether this column is nullable.
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// get/set - The maximum length of this column.
        /// </summary>
        public int MaximumLength { get; set; }

        /// <summary>
        /// get/set - The default value in the database of this column.
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// get/set - Whether this column is a computed value.
        /// </summary>
        public bool IsComputed { get; set; }

        /// <summary>
        /// get/set - Whether this column is an identity column.
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// get/set - Whether this column is deterministic.
        /// </summary>
        public bool IsDeterministic { get; set; }

        /// <summary>
        /// get/set - The precision of this column.
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// get/set - The scale of this column.
        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// get - Whether this column belongs to a primary key constraint.
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                return this.Constraints.Count(c => c.ConstraintType == ConstraintType.PrimaryKey && c.Columns.Count(cl => cl.Name.Equals(this.Name)) == 1) == 1;
            }
        }

        /// <summary>
        /// get - Whether this column belongs to a foreign key constraint.
        /// </summary>
        public bool IsForeignKey
        {
            get
            {
                return this.Constraints.Count(c => c.ConstraintType == ConstraintType.ForeignKey && c.Columns.Count(cl => cl.Name.Equals(this.Name)) == 1) != 0;
            }
        }

        /// <summary>
        /// get - Whether this column belongs to a unique constraint.
        /// </summary>
        public bool IsUnique
        {
            get
            {
                return this.Constraints.Count(c => c.ConstraintType == ConstraintType.Unique && c.Columns.Count(cl => cl.Name.Equals(this.Name)) == 1) != 0;
            }
        }

        /// <summary>
        /// get - Collection of Constraint objects.
        /// </summary>
        public ConstraintCollection Constraints { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Column object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameters 'name' and 'columnType' cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameters 'name' and 'columnType' cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter 'ordinalPosition' must be equal to or greater than 0.</exception>
        /// <param name="name">A unique name to identify the column.</param>
        /// <param name="dbType">The original database type of the column.</param>
        /// <param name="ordinalPosition">The ordinal position of the column within the entity.</param>
        public Column(string name, string dbType, int ordinalPosition)
        {
            Assert.IsNotNullOrEmpty(name, "name");
            Assert.IsNotNullOrEmpty(dbType, "dbType");
            Assert.MinRange(ordinalPosition, 0, "ordinalPosition");

            this.Name = name;
            this.DbType = dbType;
            this.NativeType = GetNativeType(dbType);
            this.OrdinalPosition = ordinalPosition;
            this.Constraints = new ConstraintCollection();
        }
        #endregion

        #region Methods
        /// <summary>
        /// The unique hash code to identify this column.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            return Fosol.Common.HashCode.Create(this.Name).Merge(this.OrdinalPosition).Merge(this.DbType);
        }

        /// <summary>
        /// Set the 'NativeType' property value based on the 'columnType' value.
        /// Override this method in your class to ensure valid type conversion occurs.
        /// </summary>
        /// <param name="columnType">The original database column type name.</param>
        protected abstract Type GetNativeType(string columnDbType);

        /// <summary>
        /// Converts the 'Default' property value into the 'NativeType'.
        /// You should override this method in your database specific type Column so that you can handle other default values.
        /// </summary>
        /// <returns>Default value in the 'NativeType'.</returns>
        public virtual object GetDefaultValue()
        {
            if (!string.IsNullOrEmpty(this.Default))
            {
                if (this.NativeType == typeof(String))
                    return this.Default;
                else
                {
                    try
                    {
                        // Attempt to convert the default value into the native .NET type.
                        // Many default values in the database are not convertable because they are database method types.
                        // If the default value is a database method it will simply fail and return null.
                        return Convert.ChangeType(this.Default, this.NativeType);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Perform shallow clone of this object.
        /// </summary>
        /// <returns>A cloned copy of this object.</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Perform shallow clone of this object.
        /// </summary>
        /// <returns>A cloned copy of this object.</returns>
        public abstract Column Clone();
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
