﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralStocks.SqlDatabase;

namespace NeuralStocksTests
{
    [TestClass]
    public class SqlDatabaseManagerTest
    {
        [TestMethod]
        public void TestImplementsInterface()
        {
            var interfaces = typeof (SqlDatabaseManager).GetInterfaces();
            Assert.AreEqual(1, interfaces.Count());
            Assert.IsTrue(interfaces.Contains(typeof (ISqlDatabaseManager)));
        }
    }
}