﻿using System.Collections.Generic;
using NeuralStocks.DatabaseLayer.Model.StockApi;

namespace NeuralStocks.DatabaseLayer.Communicator.StockApi
{
    public interface IStockMarketApiCommunicator
    {
        List<CompanyLookupResponse> CompanyLookup(CompanyLookupRequest request);
        QuoteLookupResponse QuoteLookup(QuoteLookupRequest lookupRequest);
    }
}