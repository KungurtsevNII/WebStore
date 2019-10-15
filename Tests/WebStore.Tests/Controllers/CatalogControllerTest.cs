using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.Controllers;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTest
    {
        [TestMethod]
        public void ProductDetails_Returns_NotFound()
        {
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns(default(ProductDTO));

            var controller = new CatalogController(productDataMock.Object);

            var result = controller.ProductDetails(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        public void Shop_Method_Return_Correct_View()
        {
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
                .Returns<ProductFilter>(filter => new[] {
                    new ProductDTO
                    {
                        Id = 1,
                        Name = "Product 1",
                    },
                    new ProductDTO
                    {
                        Id = 2,
                        Name = "Product 2"
                    }
                });

            var controller = new CatalogController(productDataMock.Object);

            const int expected_section_id = 1;
            const int expected_brand_id = 5;

            var result = controller.Shop(expected_section_id, expected_brand_id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CatalogViewModel>(viewResult.ViewData.Model);

            Assert.Equal(2, model.Products.Count());
            Assert.Equal(expected_brand_id, model.BrandId);
            Assert.Equal(expected_section_id, model.SectionId);
        }
    }
}
