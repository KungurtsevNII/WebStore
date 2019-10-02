using System;
using System.Collections.Generic;
using System.Text;
using Assert = Xunit.Assert;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Interfaces.Api;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebApiTestControllerTests
    {
        private WebApiTestController _controller;
        private string[] _expectedValues = { "1", "2", "3" };

        [TestInitialize]
        public void Initialize()
        {
            var valueServiceMock = new Mock<IValuesService>();
            var expectedValues = new[] { "1", "2", "3" };
            valueServiceMock
                .Setup(service => service.GetAsync())
                .ReturnsAsync(_expectedValues);

            _controller = new WebApiTestController(valueServiceMock.Object);
        }

        [TestMethod]
        public async Task Index_Method_Returns_View_With_Values()
        {
            var result = await _controller.Index();
            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<string>>(view_result.Model);

            Assert.Equal(_expectedValues.Length, model.Count());
        }
    }
}
