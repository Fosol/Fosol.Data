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
        /// get - A collection of tables within this model.
        /// </summary>
        public EntityCollection<Table> Tables { get; private set; }

        /// <summary>
        /// get - A collection of views within this model.
        /// </summary>
        public EntityCollection<View> Views { get; private set; }

        /// <summary>
        /// get - A collection of routines within this model.
        /// </summary>
        public EntityCollection<Routine> Routines { get; private set; }
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
            this.Tables = new EntityCollection<Table>();
            this.Views = new EntityCollection<View>();
            this.Routines = new EntityCollection<Routine>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determines if the table with the specified name already exists within the model.
        /// </summary>
        /// <param name="name">Name to identify the table.</param>
        /// <returns>True if the model already contains an table with the specified name.</returns>
        public bool ContainsTable(string name)
        {
            return this.Tables.ContainsEntity(name);
        }
        
        /// <summary>
        /// Determines if the view with the specified name already exists within the model.
        /// </summary>
        /// <param name="name">Name to identify the view.</param>
        /// <returns>True if the model already contains an view with the specified name.</returns>
        public bool ContainsView(string name)
        {
            return this.Views.ContainsEntity(name);
        }

        /// <summary>
        /// Determines if the routine with the specified name already exists within the model.
        /// </summary>
        /// <param name="name">Name to identify the routine.</param>
        /// <returns>True if the model already contains an routine with the specified name.</returns>
        public bool ContainsRoutine(string name)
        {
            return this.Routines.ContainsEntity(name);
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
