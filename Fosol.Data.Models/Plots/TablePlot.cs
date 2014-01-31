using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models.Plots
{
    public sealed class TablePlot
        : Plot<Table, Configuration.TableElement>
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public TablePlot(Configuration.ConventionElement conventions, Table table, Configuration.TableElement config)
            : base(conventions, table, config)
        {

        }
        #endregion

        #region Methods
        public override string GetName()
        {
            if (this.Config != null
                && !string.IsNullOrEmpty(this.Config.Alias))
            {
                return this.Config.Alias;
            }

            return this.Conventions.CreateAlias(this.Entity.Name);
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
