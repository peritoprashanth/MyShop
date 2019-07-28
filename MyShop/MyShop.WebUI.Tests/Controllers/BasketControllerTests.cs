using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Services;
using MyShop.WebUI.Controllers;
using MyShop.WebUI.Tests.Mocks;
using System.Web.Mvc;
using MyShop.Core.ViewModels;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTests
    {
        [TestMethod]
        public void CanAddBasketItems()
        {
            // Arrange             
            IRepository<Product> products = new Mocks.MockContext<Product>();
            IRepository<Basket> baskets = new Mocks.MockContext<Basket>();

            var httpContext = new MockHttpContext();


            IBasketService basketService = new BasketService(products, baskets);

            // Act 
            //basketService.AddToBasket(httpContext, "123");
            var controller = new BasketController(basketService);
            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            controller.AddToBasket("123");

            Basket basket = baskets.Collection().FirstOrDefault();

            // Assert
            Assert.IsNotNull(basket);
            Assert.AreEqual(1, basket.BasketItems.Count);
            Assert.AreEqual("123", basket.BasketItems.FirstOrDefault().ProductId);
            
        }

        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            // Arrange             
            IRepository<Product> products = new Mocks.MockContext<Product>();
            IRepository<Basket> baskets = new Mocks.MockContext<Basket>();

            products.Insert(new Product() { Id = "123", Price = 10 });
            products.Insert(new Product() { Id = "222", Price = 5 });

            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() {ProductId = "123", Quantity = 2 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "222", Quantity = 1 });

            baskets.Insert(basket);

            IBasketService basketService = new BasketService(products, baskets);

            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket") { Value = basket.Id });

            var controller = new BasketController(basketService);
            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel)result.ViewData.Model;


            Assert.AreEqual( 3 , basketSummary.BasketCount);
            Assert.AreEqual( 25 , basketSummary.BasketTotal);         


        }
    }
}
