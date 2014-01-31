using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models
{
    public sealed class Routine
        : Entity
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public Routine(string name, string entityType = "ROUTINE")
            : base(name, entityType)
        {

        }
        #endregion

        #region Methods
        public override Entity Clone()
        {
            var routine = new Routine(this.Name, this.EntityType)
            {
                Alias = this.Alias,
                Catalog = this.Catalog,
                Schema = this.Schema
            };

            foreach (var column in this.Columns)
            {
                routine.Columns.Add(column.Clone());
            }

            foreach (var constraint in this.Constraints)
            {
                routine.Constraints.Add(constraint.Clone());
            }

            return routine;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
