﻿using System.Collections.Generic;
using System.Data.SQLite;
using NeuralStocks.Backend.ApiCommunication;

namespace NeuralStocks.Backend.Database
{
    public interface IDatabaseCommunicator
    {
        void CreateDatabase(string databaseName);
        void CreateCompanyTable(SQLiteConnection connection);
        void AddCompanyToTable(SQLiteConnection connection, CompanyLookupResponse company);
        void UpdateCompanyTimestamp(SQLiteConnection connection, QuoteLookupResponse response);
        List<QuoteLookupRequest> GetQuoteLookupsFromTable(SQLiteConnection connection);
        void AddQuoteResponseToTable(SQLiteConnection connection, QuoteLookupResponse response);
    }
}