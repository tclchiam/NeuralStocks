﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralStocks.Backend.ApiCommunication;
using NeuralStocks.Backend.Tests.Testing;
using NeuralStocks.Frontend.Controller;
using NeuralStocks.Frontend.Launcher;
using NeuralStocks.Frontend.UI;

namespace NeuralStocks.Frontend.Tests.Launcher
{
    [TestClass]
    public class NeuralStocksFrontendLauncherTest : AssertTestClass
    {
        [TestMethod]
        public void TestImplementsInterface()
        {
            AssertImplementsInterface(typeof (INeuralStocksFrontendLauncher), typeof (NeuralStocksFrontendLauncher));
        }

        [TestMethod]
        public void TestFrontEndControllerIsConstructedWithCorrectArguments()
        {
            var launcher = new NeuralStocksFrontendLauncher();
            var frontendController = AssertIsOfTypeAndGet<FrontendController>(launcher.FrontendController);

            var stockCommunicator =
                AssertIsOfTypeAndGet<StockMarketApiCommunicator>(frontendController.StockCommunicator);
            Assert.AreSame(StockMarketApi.Singleton, stockCommunicator.StockMarketApi);
            Assert.AreSame(TimestampParser.Singleton, stockCommunicator.TimestampParser);

            Assert.AreSame(DataTableFactory.Factory, frontendController.TableFactory);
        }

        [TestMethod]
        public void TestMainWindowIsConstructedWithFrontendController()
        {
            var launcher = new NeuralStocksFrontendLauncher();
            var mainWindow = AssertIsOfTypeAndGet<MainWindow>(launcher.MainWindow);

            Assert.AreSame(launcher.FrontendController, mainWindow.FrontendController);
        }
    }
}