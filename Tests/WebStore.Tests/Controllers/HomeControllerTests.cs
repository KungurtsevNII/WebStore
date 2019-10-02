using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Controllers;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController _controller;

        [TestInitialize]
        public void Initialize()
        {
            var loggerMoq = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(loggerMoq.Object);
        }

        [TestCleanup]
        public void CleanUp() { }

        [TestMethod]
        public void Index_Returns_View()
        {
            var result = _controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void Error404_Throw_ApplicationException()
        {
            var result = _controller.NotFoundPage();
            Assert.IsType<ViewResult>(result);
        }

        [TestMethod, ExpectedException(typeof(ApplicationException))]
        public void ThrowException_Throw_NewApplicationException()
        {
            _ = _controller.ThrowException();
        }

        [TestMethod]
        public void ErrorStatus_404_Redirect_To_NotFoundPage()
        {
            var result = _controller.ErrorStatus("404");
            var redirectToAction = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToAction.ControllerName);
            Assert.Equal(nameof(HomeController.NotFoundPage), redirectToAction.ActionName);
        }
    }
}
