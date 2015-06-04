﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralStocks.ApiCommunication;

namespace NeuralStocksTests.ApiCommunication
{
    [TestClass]
    public class CompanyLookupRequestTest
    {
        [TestMethod]
        public void TestCompanyLookupRequestConstructedWithCorrectCompany()
        {
            const string company1 = "NFLX";
            const string company2 = "AAPL";

            var companyLookupRequest1 = new CompanyLookupRequest(company1);
            var companyLookupRequest2 = new CompanyLookupRequest(company2);

            Assert.AreEqual(company1, companyLookupRequest1.Company);
            Assert.AreEqual(company2, companyLookupRequest2.Company);
        }
    }
}