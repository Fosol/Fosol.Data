using Fosol.Common.Validation;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models
{
    /// <summary>
    /// A Model represents a data structure from a database.
    /// You can automate the process of creating a Model by using the appropriate ModelFactory for your database.
    /// </summary>
    public class Model
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - A unique name to identify the model.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// get/set - A unique name to identify the model (used instead of the 'name' property).
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// get - A collection of entities within this model.
        /// </summary>
        public EntityCollection Entities { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Model object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter 'name' cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter 'name' cannot be null.</exception>
        /// <param name="name">Name to identify the model.</param>
        public Model(string name)
        {
            Assert.IsNotNullOrEmpty(name, "name");

            this.Name = name;
            this.Entities = new EntityCollection();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determines if the entity with the specified name already exists within the model.
        /// </summary>
        /// <param name="name">Name to identify the entity.</param>
        /// <returns>True if the model already contains an entity with the specified name.</returns>
        public bool ContainsEntity(string name)
        {
            return this.Entities.ContainsEntity(name);
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
        #endregion

        #region Events
        #endregion
    }
}
