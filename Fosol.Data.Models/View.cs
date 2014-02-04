using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models
{
    public sealed class View
        : Entity
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public View(string name, string entityType = "VIEW")
            : base(name, entityType)
        {

        }
        #endregion

        #region Methods
        public override Entity Clone()
        {
            var view = new View(this.Name, this.EntityType)
            {
                Alias = this.Alias,
                Catalog = this.Catalog,
                Schema = this.Schema
            };

            DeepClone(view);

            return view;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
