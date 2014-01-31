using Fosol.Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models
{
    /// <summary>
    /// An EntityCollection contains a collection of Entity objects that represent tables and views.
    /// </summary>
    public sealed class EntityCollection<T>
        : IEnumerable<T>, ICloneable
        where T : Entity
    {
        #region Variables
        private Dictionary<string, T> _Entities = new Dictionary<string, T>();
        #endregion

        #region Properties
        /// <summary>
        /// get - Number of entity objects within the collection.
        /// </summary>
        public int Count { get { return _Entities.Count; } }

        /// <summary>
        /// get - The entity with the specified name.
        /// </summary>
        /// <param name="name">Name to identify the entity.</param>
        /// <returns>A entity if it exists.</returns>
        public T this[string name]
        {
            get { return _Entities[name]; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an EntityCollection.
        /// Makes it impossible to use the EntityColection externally.
        /// </summary>
        internal EntityCollection()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the enumerator for this EntityCollection.
        /// </summary>
        /// <returns>IEnumerator object.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var entity in _Entities.Select(e => e.Value))
            {
                yield return entity;
            }
        }

        /// <summary>
        /// Get the enumerator for this EntityCollection.
        /// </summary>
        /// <returns>IEnumerator object.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Add the entity to the collection.
        /// </summary>
        /// <param name="entity">Entity object to add.</param>
        public void Add(T entity)
        {
            Assert.IsNotNull(entity, "entity");

            _Entities.Add(entity.Name, entity);
        }

        /// <summary>
        /// Merge the specified collection into this EntityCollection.
        /// </summary>
        /// <param name="entities">EntityCollection object.</param>
        public void Merge(EntityCollection<T> entities)
        {
            Assert.IsNotNull(entities, "entities");

            foreach (var entity in entities)
            {
                _Entities.Add(entity.Name, entity);
            }
        }

        /// <summary>
        /// Remove the entity from the collection.
        /// </summary>
        /// <param name="name">Name to identify the entity.</param>
        /// <returns>True if the entity was removed.</returns>
        public bool Remove(string name)
        {
            Assert.IsNotNullOrEmpty(name, "name");

            return _Entities.Remove(name);
        }

        /// <summary>
        /// Removes the entity from the collection.
        /// </summary>
        /// <param name="entity">Entity object to remove from the collection.</param>
        /// <returns>True if the entity was removed.</returns>
        public bool Remove(T entity)
        {
            Assert.IsNotNull(entity, "entity");

            return _Entities.Remove(entity.Name);
        }

        /// <summary>
        /// Determines whether the collection contains an entity with the specified name.
        /// </summary>
        /// <param name="name">Name to identify the entity.</param>
        /// <returns>True of the entity exists with the specified name.</returns>
        public bool ContainsEntity(string name)
        {
            return _Entities.ContainsKey(name);
        }

        /// <summary>
        /// Deep clone the collection.
        /// </summary>
        /// <returns>A new copy of the collection.</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Deep clone the collection.
        /// </summary>
        /// <returns>A new copy of the collection.</returns>
        public EntityCollection<T> Clone()
        {
            var collection = new EntityCollection<T>();

            foreach (var entity in this)
            {
                collection.Add((T)entity.Clone());
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
