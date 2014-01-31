using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models
{
    public sealed class Table
        : Entity
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public Table(string name, string entityType = "TABLE")
            : base(name, entityType)
        {

        }
        #endregion

        #region Methods
        public override Entity Clone()
        {
            var table = new Table(this.Name, this.EntityType)
            {
                Alias = this.Alias,
                Catalog = this.Catalog,
                Schema = this.Schema
            };

            foreach (var column in this.Columns)
            {
                table.Columns.Add(column.Clone());
            }

            foreach (var constraint in this.Constraints)
            {
                table.Constraints.Add(constraint.Clone());
            }

            return table;
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
