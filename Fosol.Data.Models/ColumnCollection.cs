using Fosol.Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models
{
    /// <summary>
    /// A ColumnCollection contains a collection of columns.
    /// It also provides a way to get columns based on their name or their ordinal position.
    /// </summary>
    public sealed class ColumnCollection
        : IEnumerable<Column>
    {
        #region Variables
        private System.Threading.ReaderWriterLockSlim _Lock = new System.Threading.ReaderWriterLockSlim();
        private Dictionary<string, Column> _Columns = new Dictionary<string, Column>();
        private SortedDictionary<int, string> _ColumnNames = new SortedDictionary<int, string>();
        #endregion

        #region Properties
        /// <summary>
        /// get - The number of columns in the collection.
        /// </summary>
        public int Count { get { return _Columns.Count; } }

        /// <summary>
        /// get - The column with the specified name.
        /// </summary>
        /// <param name="name">The name to identify the column.</param>
        /// <returns>Column object if it exists.</returns>
        public Column this[string name]
        {
            get { return _Columns[name]; }
        }

        /// <summary>
        /// get - The column at the specified index position.
        /// </summary>
        /// <exception cref="System.IndexOutOfRangeException">Index position specified is outside of the available range.</exception>
        /// <param name="index">Index position of the column.</param>
        /// <returns>Column object if it exists.</returns>
        public Column this[int index]
        {
            get 
            {
                if (index < 0 || index >= this.Count)
                    throw new IndexOutOfRangeException();

                return _Columns[_ColumnNames[index]]; 
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ColumnCollection object.
        /// Makes it impossible for external code to create a ColumnCollection.
        /// </summary>
        internal ColumnCollection()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Determine if the collection contains a Column with the specified name.
        /// </summary>
        /// <param name="name">Name to identify the column.</param>
        /// <returns>True if the column exists.</returns>
        public bool ContainsColumn(string name)
        {
            return _Columns.ContainsKey(name);
        }

        /// <summary>
        /// Add a Column object to this collection.
        /// The column must not already exist within the collection (the name and ordinal position must be unique).
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter 'column' cannot be null.</exception>
        /// <exception cref="System.InvalidOperationException">Cannot add a column that already exists within the collection.</exception>
        /// <param name="column">Column object to add to collection.</param>
        public void Add(Column column)
        {
            Assert.IsNotNull(column, "column");

            _Lock.EnterUpgradeableReadLock();
            try
            {
                if (_Columns.ContainsKey(column.Name))
                    throw new InvalidOperationException(string.Format("Column '{0}' already exists.  Cannot add a duplicate column.", column.Name));

                if (_ColumnNames.ContainsKey(column.OrdinalPosition))
                    throw new InvalidOperationException(string.Format("Column at ordinal position '{0}' already exists.  Cannot add a duplicate column.", column.OrdinalPosition));

                _Lock.EnterWriteLock();
                try
                {
                    _ColumnNames.Add(column.OrdinalPosition, column.Name);
                    _Columns.Add(column.Name, column);
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
            finally
            {
                _Lock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Remove the specified column from the collection.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'name' cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter 'name' cannnot be null.</exception>
        /// <param name="name">The column name.</param>
        /// <returns>True if the column with the specified name was removed.</returns>
        public bool Remove(string name)
        {
            Assert.IsNotNullOrEmpty(name, "name");

            _Lock.EnterUpgradeableReadLock();
            try
            {
                if (!_Columns.ContainsKey(name))
                    return false;

                _Lock.EnterWriteLock();
                try
                {
                    var column = _Columns[name];
                    _ColumnNames.Remove(column.OrdinalPosition);
                    return _Columns.Remove(name);
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
            finally
            {
                _Lock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Remove the column at the specified index position from the collection.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter 'index' must be equal to or greater than 0.</exception>
        /// <param name="index">Index position of the column to remove.</param>
        /// <returns>True if the column at the specified position was removed.</returns>
        public bool Remove(int index)
        {
            Assert.MinRange(index, 0, "index");

            _Lock.EnterUpgradeableReadLock();
            try
            {
                if (!_ColumnNames.ContainsKey(index))
                    return false;

                _Lock.EnterWriteLock();
                try
                {
                    var column_name = _ColumnNames[index];
                    _ColumnNames.Remove(index);
                    return _Columns.Remove(column_name);
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
            finally
            {
                _Lock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Get the enumerator for this collection.
        /// </summary>
        /// <returns>IEnumerator object.</returns>
        public IEnumerator<Column> GetEnumerator()
        {
            foreach (var column in _Columns.Select(c => c.Value))
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
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
