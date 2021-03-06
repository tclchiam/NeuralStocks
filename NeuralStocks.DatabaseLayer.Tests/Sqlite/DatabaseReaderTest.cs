﻿using System.Data.SQLite;
using System.IO;
using NeuralStocks.DatabaseLayer.Sqlite;
using NeuralStocks.DatabaseLayer.Tests.Testing;
using NUnit.Framework;

namespace NeuralStocks.DatabaseLayer.Tests.Sqlite
{
    [TestFixture]
    public class DatabaseReaderTest : AssertTestClass
    {
        private const string DatabaseFileName = "TestsStocksDatabase.sqlite";
        private const string DatabaseConnectionString = "Data Source=" + DatabaseFileName + ";Version=3;";

        [Test]
        [Category("Database")]
        public void TestGetWrappedReader()
        {
            if (File.Exists(DatabaseFileName)) File.Delete(DatabaseFileName);
            Assert.IsFalse(File.Exists(DatabaseFileName));
            SQLiteConnection.CreateFile(DatabaseFileName);
            Assert.IsTrue(File.Exists(DatabaseFileName));

            var connection = new SQLiteConnection(DatabaseConnectionString);
            var command = new SQLiteCommand("SELECT * FROM sqlite_master", connection);

            connection.Open();
            var reader = command.ExecuteReader();
            command.Dispose();
            connection.Close();

            var databaseReader = new DatabaseReader(reader);

            Assert.AreSame(reader, databaseReader.WrappedReader);
        }

        [Test]
        [Category("Database")]
        public void TestImplementsInterface()
        {
            AssertImplementsInterface(typeof (IDatabaseReader), typeof (DatabaseReader));
        }
    }
}