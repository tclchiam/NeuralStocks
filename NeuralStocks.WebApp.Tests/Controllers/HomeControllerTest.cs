﻿using System.Web.Mvc;
using NeuralStocks.WebApp.Controllers;
using NUnit.Framework;

namespace NeuralStocks.WebApp.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        [Category("Web App")]
        public void About()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [Test]
        [Category("Web App")]
        public void Contact()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [Category("Web App")]
        public void Index()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}