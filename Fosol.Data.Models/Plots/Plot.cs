using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models.Plots
{
    /// <summary>
    /// Provides a way to plot an entity and its configuration details.
    /// </summary>
    /// <typeparam name="ET">Entity object type.</typeparam>
    /// <typeparam name="CT">ConfigurationElement object type.</typeparam>
    public abstract class Plot<ET, CT>
        where ET : Entity
        where CT : System.Configuration.ConfigurationElement
    {
        #region Variables
        #endregion

        #region Properties
        /// <summary>
        /// get - The entity object.
        /// </summary>
        public ET Entity { get; private set; }

        /// <summary>
        /// get - The ConfigurationElement use to modify the entity.
        /// </summary>
        public CT Config { get; private set; }

        /// <summary>
        /// get - ConventionElement object used to control the entities values.
        /// </summary>
        public Configuration.ConventionElement Conventions { get; private set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a Plot struct.
        /// </summary>
        /// <param name="conventions">ConventionElement object.</param>
        /// <param name="entity">Entity object.</param>
        /// <param name="config">ConfigurationElement object.</param>
        public Plot(Configuration.ConventionElement conventions, ET entity, CT config)
        {
            this.Conventions = conventions;
            this.Entity = entity;
            this.Config = config;
        }
        #endregion

        #region Methods
        public abstract string GetName();

        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
