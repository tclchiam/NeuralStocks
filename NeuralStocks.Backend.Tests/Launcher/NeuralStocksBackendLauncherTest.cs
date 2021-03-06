﻿using System;
using System.IO;
using Moq;
using NeuralStocks.Backend.Controller;
using NeuralStocks.Backend.Launcher;
using NeuralStocks.DatabaseLayer.Database;
using NeuralStocks.DatabaseLayer.Sqlite;
using NeuralStocks.DatabaseLayer.StockApi;
using NeuralStocks.DatabaseLayer.Tests.Testing;
using NUnit.Framework;

namespace NeuralStocks.Backend.Tests.Launcher
{
    [TestFixture]
    public class NeuralStocksBackendLauncherTest : AssertTestClass
    {
        [Test]
        [Category("Backend")]
        public void TestConstructorSetsUpBackendControllerCorrectly()
        {
            var launcher = new NeuralStocksBackendLauncher();

            var backendController = AssertIsOfTypeAndGet<BackendController>(launcher.BackendController);

            Assert.AreSame(StockMarketApiCommunicator.Singleton, backendController.StockCommunicator);

            var communicator = AssertIsOfTypeAndGet<DatabaseCommunicator>(backendController.DatabaseCommunicator);
            Assert.AreSame(DatabaseCommandStringFactory.Singleton, communicator.Factory);

            var connection = AssertIsOfTypeAndGet<DatabaseConnection>(communicator.Connection);
            var databaseName = AssertIsOfTypeAndGet<DatabaseName>(connection.DatabaseName);
            Assert.AreEqual(DatabaseConfiguration.FullDatabaseFileName, databaseName.Name);
        }

        [Test]
        [Category("Backend")]
        public void TestConstructorSetsUpBackendLockCorrectly()
        {
            var launcher = new NeuralStocksBackendLauncher();

            var backendLock = AssertIsOfTypeAndGet<BackendLock>(launcher.BackendLock);

            Assert.AreEqual(58525, backendLock.Port);
        }

        [Test]
        [Category("Backend")]
        public void TestConstructorSetsUpSetupManagerCorrectly()
        {
            var launcher = new NeuralStocksBackendLauncher();

            var setupManager = AssertIsOfTypeAndGet<DatabaseSetupManager>(launcher.SetupManager);
            var communicator = AssertIsOfTypeAndGet<DatabaseCommunicator>(setupManager.DatabaseCommunicator);
            Assert.AreSame(DatabaseCommandStringFactory.Singleton, communicator.Factory);

            var connection = AssertIsOfTypeAndGet<DatabaseConnection>(communicator.Connection);
            var databaseName = AssertIsOfTypeAndGet<DatabaseName>(connection.DatabaseName);
            Assert.AreEqual(DatabaseConfiguration.FullDatabaseFileName, databaseName.Name);
        }

        [Test]
        [Category("Backend")]
        public void TestImplementsInterface()
        {
            AssertImplementsInterface(typeof (INeuralStocksBackendLauncher), typeof (NeuralStocksBackendLauncher));
        }

        [Test]
        [Category("Backend")]
        public void TestStartBackendCallsInitializeDatabaseOnSetupManager_DatabaseDoesNotExist()
        {
            File.Delete(TestDatabaseName);
            Assert.IsFalse(File.Exists(TestDatabaseName));

            var mockSetupManager = new Mock<IDatabaseSetupManager>();
            var mockController = new Mock<IBackendController>();
            var mockBackendLock = new Mock<IBackendLock>();
            mockBackendLock.Setup(m => m.Lock()).Returns(true);

            var launcher = new NeuralStocksBackendLauncher
            {
                SetupManager = mockSetupManager.Object,
                BackendController = mockController.Object,
                BackendLock = mockBackendLock.Object
            };

            AssertFieldIsOfTypeAndSet(launcher, "_databaseFileName", TestDatabaseName);

            mockSetupManager.Verify(m => m.InitializeDatabase(It.IsAny<string>()), Times.Never);

            launcher.StartBackend();

            mockSetupManager.Verify(m => m.InitializeDatabase(TestDatabaseName), Times.Once);
            mockBackendLock.VerifyAll();
        }

        [Test]
        [Category("Backend")]
        public void TestStartBackendCallsStartTimerOnBackendController()
        {
            var mockSetupManager = new Mock<IDatabaseSetupManager>();
            var mockController = new Mock<IBackendController>();
            var mockBackendLock = new Mock<IBackendLock>();
            mockBackendLock.Setup(m => m.Lock()).Returns(true);

            var launcher = new NeuralStocksBackendLauncher
            {
                SetupManager = mockSetupManager.Object,
                BackendController = mockController.Object,
                BackendLock = mockBackendLock.Object
            };

            mockController.Verify(m => m.StartTimer(), Times.Never);

            launcher.StartBackend();

            mockController.Verify(m => m.StartTimer(), Times.Once);
            mockBackendLock.VerifyAll();
        }

        [Test]
        [Category("Backend")]
        public void TestStartBackendDoesNotCallInitializeDatabase_StartTimer_OrWriteToConsole_BackendAlreadyLocked()
        {
            var mockWriter = new Mock<TextWriter>();
            Console.SetOut(mockWriter.Object);

            var mockSetupManager = new Mock<IDatabaseSetupManager>();
            var mockController = new Mock<IBackendController>();
            var mockBackendLock = new Mock<IBackendLock>();

            mockBackendLock.Setup(m => m.Lock()).Returns(false);

            var launcher = new NeuralStocksBackendLauncher
            {
                SetupManager = mockSetupManager.Object,
                BackendController = mockController.Object,
                BackendLock = mockBackendLock.Object
            };

            mockSetupManager.Verify(m => m.InitializeDatabase(It.IsAny<string>()), Times.Never);
            mockController.Verify(m => m.StartTimer(), Times.Never);
            mockWriter.Verify(m => m.WriteLine(It.IsAny<string>()), Times.Never);

            launcher.StartBackend();

            mockSetupManager.Verify(m => m.InitializeDatabase(It.IsAny<string>()), Times.Never);
            mockController.Verify(m => m.StartTimer(), Times.Never);
            mockWriter.Verify(m => m.WriteLine("Backend Started"), Times.Never);
            mockBackendLock.VerifyAll();
        }

        [Test]
        [Category("Backend")]
        public void TestStartBackendDoesNotCallInitializeDatabaseOnSetupManager_DatabaseExists()
        {
            File.Create(TestDatabaseName);
            Assert.IsTrue(File.Exists(TestDatabaseName));

            var mockSetupManager = new Mock<IDatabaseSetupManager>();
            var mockController = new Mock<IBackendController>();
            var mockBackendLock = new Mock<IBackendLock>();
            mockBackendLock.Setup(m => m.Lock()).Returns(true);

            var launcher = new NeuralStocksBackendLauncher
            {
                SetupManager = mockSetupManager.Object,
                BackendController = mockController.Object,
                BackendLock = mockBackendLock.Object
            };

            AssertFieldIsOfTypeAndSet(launcher, "_databaseFileName", TestDatabaseName);

            mockSetupManager.Verify(m => m.InitializeDatabase(It.IsAny<string>()), Times.Never);

            launcher.StartBackend();

            mockSetupManager.Verify(m => m.InitializeDatabase(It.IsAny<string>()), Times.Never);
            mockBackendLock.VerifyAll();
        }

        [Test]
        [Category("Backend")]
        public void TestStartBackendWritesCorrectlyToConsole()
        {
            var mockWriter = new Mock<TextWriter>();
            Console.SetOut(mockWriter.Object);

            var mockSetupManager = new Mock<IDatabaseSetupManager>();
            var mockController = new Mock<IBackendController>();
            var mockBackendLock = new Mock<IBackendLock>();
            mockBackendLock.Setup(m => m.Lock()).Returns(true);

            var launcher = new NeuralStocksBackendLauncher
            {
                SetupManager = mockSetupManager.Object,
                BackendController = mockController.Object,
                BackendLock = mockBackendLock.Object
            };

            mockWriter.Verify(m => m.WriteLine(It.IsAny<string>()), Times.Never);

            launcher.StartBackend();

            mockWriter.Verify(m => m.WriteLine("Backend Started"), Times.Once);
            mockBackendLock.VerifyAll();
        }

        [Test]
        [Category("Backend")]
        public void TestStartBackendWritesToConsole_BackendAlreadyLocked()
        {
            var mockWriter = new Mock<TextWriter>();
            Console.SetOut(mockWriter.Object);

            var mockSetupManager = new Mock<IDatabaseSetupManager>();
            var mockController = new Mock<IBackendController>();
            var mockBackendLock = new Mock<IBackendLock>();

            mockBackendLock.Setup(m => m.Lock()).Returns(false);

            var launcher = new NeuralStocksBackendLauncher
            {
                SetupManager = mockSetupManager.Object,
                BackendController = mockController.Object,
                BackendLock = mockBackendLock.Object
            };

            mockWriter.Verify(m => m.WriteLine(It.IsAny<string>()), Times.Never);

            launcher.StartBackend();

            mockWriter.Verify(m => m.WriteLine("Backend already started. Application is locked"), Times.Once);
            mockBackendLock.VerifyAll();
        }
    }
}