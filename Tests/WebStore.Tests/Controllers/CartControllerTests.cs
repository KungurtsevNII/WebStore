using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using WebStore.Controllers;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.Models;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public void Chekout_ModelState_Invalid_Returns_ViewModel()
        {
            var cartServiceMock = new Mock<ICartService>();
            var orderServiceMock = new Mock<IOrderService>();

            var controller = new CartController(cartServiceMock.Object, orderServiceMock.Object);

            // Добавляем ошибку
            controller.ModelState.AddModelError("error", "InvalidModel");

            const string expectedModelName = "Test order";

            var result = controller.CheckOut(new OrderViewModel
            {
                Name = expectedModelName
            });

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<DetailsViewModel>(viewResult.ViewData.Model);
            Assert.Equal(expectedModelName, model.OrderViewModel.Name);
        }

        [TestMethod]
        public void CheckOut_Calls_Service_and_Return_Redirect()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") }));

            var cart_service_mock = new Mock<ICartService>();
            cart_service_mock
               .Setup(c => c.TransformCart())
               .Returns(() => new CartViewModel
               {
                   Items = new Dictionary<ProductViewModel, int>
                    {
                        { new ProductViewModel(), 1 }
                    }
               });

            var order_service_mock = new Mock<IOrderService>();
            order_service_mock
               .Setup(c => c.CreateOrder(It.IsAny<CreateOrderModel>(), It.IsAny<string>()))
               .Returns(new OrderDTO { Id = 1 });

            var conteoler = new CartController(cart_service_mock.Object, order_service_mock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = user
                    }
                }
            };

            var result = conteoler.CheckOut(new OrderViewModel
            {
                Name = "Test",
                Address = "",
                Phone = ""
            });

            var redirect_result = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirect_result.ControllerName);
            Assert.Equal(nameof(CartController.OrderConfirmed), redirect_result.ActionName);

            Assert.Equal(1, redirect_result.RouteValues["id"]);
        }
    }
}
