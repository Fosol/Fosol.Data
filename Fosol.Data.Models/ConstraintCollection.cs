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
        : IEnumerable<Constraint>
    {
        #region Variables
        private Dictionary<string, Constraint> _Constraints = new Dictionary<string, Constraint>();
        #endregion

        #region Properties
        /// <summary>
        /// get - Number of entity objects within the collection.
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
        /// Get the enumerator for this ConstraintCollection.
        /// </summary>
        /// <returns>IEnumerator object.</returns>
        public IEnumerator<Constraint> GetEnumerator()
        {
            foreach (var entity in _Constraints.Select(e => e.Value))
            {
                yield return entity;
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
        /// Add the entity to the collection.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Parameter 'entity' cannot be null.</exception>
        /// <param name="entity">Constraint object to add.</param>
        public void Add(Constraint entity)
        {
            Assert.IsNotNull(entity, "entity");

            _Constraints.Add(entity.Name, entity);
        }

        /// <summary>
        /// Remove the entity from the collection.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'name' cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter 'name' cannot be null.</exception>
        /// <param name="name">Name to identify the entity.</param>
        /// <returns>True if the entity was removed.</returns>
        public bool Remove(string name)
        {
            Assert.IsNotNullOrEmpty(name, "name");

            return _Constraints.Remove(name);
        }

        /// <summary>
        /// Determines whether the collection contains an entity with the specified name.
        /// </summary>
        /// <param name="name">Name to identify the entity.</param>
        /// <returns>True of the entity exists with the specified name.</returns>
        public bool ContainsConstraint(string name)
        {
            return _Constraints.ContainsKey(name);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
