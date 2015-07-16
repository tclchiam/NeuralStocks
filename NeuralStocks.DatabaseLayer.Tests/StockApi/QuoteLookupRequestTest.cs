﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralStocks.DatabaseLayer.StockApi;
using NeuralStocks.DatabaseLayer.Tests.Testing;

namespace NeuralStocks.DatabaseLayer.Tests.StockApi
{
    [TestClass]
    public class QuoteLookupRequestTest : AssertTestClass
    {
        [TestMethod, TestCategory("StockApi")]
        public void TestStockQuoteRequestConstructedWithCorrectCompany()
        {
            const string company1 = "NFLX";
            const string company2 = "AAPL";
            const string timestamp1 = "Tues Jun 16";
            const string timestamp2 = "Wed Jun 17";

            var stockLookupRequest1 = new QuoteLookupRequest
            {
                Company = company1,
                Timestamp = timestamp1
            };

            var stockLookupRequest2 = new QuoteLookupRequest
            {
                Company = company2,
                Timestamp = timestamp2
            };

            Assert.AreEqual(company1, stockLookupRequest1.Company);
            Assert.AreEqual(timestamp1, stockLookupRequest1.Timestamp);
            Assert.AreEqual(company2, stockLookupRequest2.Company);
            Assert.AreEqual(timestamp2, stockLookupRequest2.Timestamp);
        }
    }
}