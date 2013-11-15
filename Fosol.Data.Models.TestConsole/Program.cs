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
            var config = Configuration.ModelFactorySection.GetDefault();
            var config_model = config.DataModels["Fosol.Team.Db.Dev"];
            var factory = new SqlModelFactory(config_model);
            var model = factory.Build();
        }
    }
}
