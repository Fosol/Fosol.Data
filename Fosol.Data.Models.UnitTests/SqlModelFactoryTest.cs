using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fosol.Data.Models.SqlClient;
using System.Data.SqlClient;

namespace Fosol.Data.Models.UnitTests
{
    [TestClass]
    public class SqlModelFactoryTest
    {
        [TestMethod]
        public void Create()
        {
            var factory = new SqlModelFactory("TestSqlDb");

            Assert.IsNotNull(factory);
            Assert.IsTrue(factory.Configuration.Connection is SqlConnection);
        }
    }
}
