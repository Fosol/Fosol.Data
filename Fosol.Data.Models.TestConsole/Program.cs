using Fosol.Data.Models.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Data.Models.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = (Configuration.ModelFactorySection)System.Configuration.ConfigurationManager.GetSection("fosol.datamodel");
            var factory = new SqlModelFactory("FosolTeamDb");
            var model = factory.Build();
        }
    }
}
