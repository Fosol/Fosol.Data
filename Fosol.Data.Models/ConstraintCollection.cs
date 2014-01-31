using Fosol.Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models
{
    /// <summary>
    /// An ConstraintCollection contains a collection of Constraint objects that represent tables and views.
    /// </summary>
    public sealed class ConstraintCollection
        : IEnumerable<Constraint>, ICloneable
    {
        #region Variables
        private Dictionary<string, Constraint> _Constraints = new Dictionary<string, Constraint>();
        #endregion

        #region Properties
        /// <summary>
        /// get - Number of constraint objects within the collection.
        /// </summary>
        public int Count { get { return _Constraints.Count; } }

        /// <summary>
        /// get - The constraint with the specified name.
        /// </summary>
        /// <param name="name">Name to identify the constraint.</param>
        /// <returns>A Constraint if it exists.</returns>
        public Constraint this[string name]
        {
            get { return _Constraints[name]; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an ConstraintCollection.
        /// Makes it impossible to use the ConstraintColection externally.
        /// </summary>
        internal ConstraintCollection()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Get a collection of keys.
        /// </summary>
        /// <returns></returns>
        public string[] GetNames()
        {
            return _Constraints.Select(c => c.Key).ToArray();
        }


        /// <summary>
        /// Get the enumerator for this ConstraintCollection.
        /// </summary>
        /// <returns>IEnumerator object.</returns>
        public IEnumerator<Constraint> GetEnumerator()
        {
            foreach (var constraint in _Constraints.Select(e => e.Value))
            {
                yield return constraint;
            }
        }

        /// <summary>
        /// Get the enumerator for this ConstraintCollection.
        /// </summary>
        /// <returns>IEnumerator object.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Add the constraint to the collection.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter 'constraint' cannot be null.</exception>
        /// <param name="constraint">Constraint object to add.</param>
        public void Add(Constraint constraint)
        {
            Assert.IsNotNull(constraint, "constraint");

            _Constraints.Add(constraint.Name, constraint);
        }

        /// <summary>
        /// Remove the constraint from the collection.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'name' cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter 'name' cannot be null.</exception>
        /// <param name="name">Name to identify the constraint.</param>
        /// <returns>True if the constraint was removed.</returns>
        public bool Remove(string name)
        {
            Assert.IsNotNullOrEmpty(name, "name");

            return _Constraints.Remove(name);
        }

        /// <summary>
        /// Remove the constraint from the collection.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter 'constraint' cannot be null.</exception>
        /// <param name="constraint">Constraint to remove from this collection.</param>
        /// <returns>True if the constraint was removed.</returns>
        public bool Remove(Constraint constraint)
        {
            Assert.IsNotNull(constraint, "constraint");

            return _Constraints.Remove(constraint.Name);
        }

        /// <summary>
        /// Determines whether the collection contains an constraint with the specified name.
        /// </summary>
        /// <param name="name">Name to identify the constraint.</param>
        /// <returns>True of the constraint exists with the specified name.</returns>
        public bool ContainsConstraint(string name)
        {
            return _Constraints.ContainsKey(name);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public ConstraintCollection Clone()
        {
            var collection = new ConstraintCollection();

            foreach (var constraint in this)
            {
                collection.Add(constraint.Clone());
            }

            return collection;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
