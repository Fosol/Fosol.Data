using Fosol.Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models
{
    /// <summary>
    /// A ConstraintColumnCollection contains a collection of column references for a Constraint.
    /// Essentially it provides a way to find the constraints associated to the column.
    /// </summary>
    public sealed class ConstraintColumnCollection
        : IEnumerable<ConstraintColumn>
    {
        #region Variables
        private List<ConstraintColumn> _Columns = new List<ConstraintColumn>();
        #endregion

        #region Properties
        /// <summary>
        /// get - Number of columns in this constraint.
        /// </summary>
        public int Count { get { return _Columns.Count; } }

        /// <summary>
        /// get - The constraint at the specified index position.
        /// </summary>
        /// <exception cref="System.IndexOutOfRangeException">Parameter 'index' must be a valid index position.</exception>
        /// <param name="index">Index position of the desired constraint.</param>
        /// <returns>The ConstraintColumn at the specified index position.</returns>
        public ConstraintColumn this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                    throw new IndexOutOfRangeException();

                return _Columns[index];
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ConstraintColumnCollection object.
        /// Ensures this object cannot be created externally.
        /// </summary>
        internal ConstraintColumnCollection()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the enumerator for this collection.
        /// </summary>
        /// <returns>IEnumerator object.</returns>
        public IEnumerator<ConstraintColumn> GetEnumerator()
        {
            foreach (var column in _Columns)
            {
                yield return column;
            }
        }

        /// <summary>
        /// Get the enumerator for this collection.
        /// </summary>
        /// <returns>IEnumerator object.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Add the column to the collection.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter 'column' cannot be null.</exception>
        /// <param name="column">ConstraintColumn object to add.</param>
        public void Add(ConstraintColumn column)
        {
            Assert.IsNotNull(column, "column");

            if (_Columns.Count(c => c.Name.Equals(column.Name)) != 0)
                throw new InvalidOperationException(string.Format("Column '{0}' already exists.  Cannot add a duplicate column.", column.Name));

            _Columns.Add(column);
        }

        /// <summary>
        /// Remove the column with the specified name from the collection.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'name' cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter 'name' cannot be null.</exception>
        /// <param name="name">Name to identify the column.</param>
        public void Remove(string name)
        {
            Assert.IsNotNullOrEmpty(name, "name");

            var column = _Columns.FirstOrDefault(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (column != null)
                _Columns.Remove(column);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
