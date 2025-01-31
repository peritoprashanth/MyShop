﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Services;
using MyShop.WebUI.Controllers;
using MyShop.WebUI.Tests.Mocks;
using System.Web.Mvc;
using MyShop.Core.ViewModels;
using System.Security.Principal;

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
            IRepository<Order> orders = new Mocks.MockContext<Order>();
            IRepository<Customer> customers = new Mocks.MockContext<Customer>();

            var httpContext = new MockHttpContext();


            IBasketService basketService = new BasketService(products, baskets);
            IOrderService orderService = new OrderService(orders);

            // Act 
            //basketService.AddToBasket(httpContext, "123");
            var controller = new BasketController(basketService, orderService, customers);
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
            IRepository<Order> orders = new Mocks.MockContext<Order>();
            IRepository<Customer> customers = new Mocks.MockContext<Customer>();

            products.Insert(new Product() { Id = "123", Price = 10 });
            products.Insert(new Product() { Id = "222", Price = 5 });

            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "123", Quantity = 2 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "222", Quantity = 1 });

            baskets.Insert(basket);

            IBasketService basketService = new BasketService(products, baskets);
            IOrderService orderService = new OrderService(orders);

            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket") { Value = basket.Id });

            var controller = new BasketController(basketService, orderService, customers);
            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel)result.ViewData.Model;


            Assert.AreEqual(3, basketSummary.BasketCount);
            Assert.AreEqual(25, basketSummary.BasketTotal);

        }

        [TestMethod]
        public void CanCheckoutAndCreateOrder()
        {
            // Arrange           

            IRepository<Product> products = new MockContext<Product>();
            IRepository<Customer> customers = new Mocks.MockContext<Customer>();
            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 5.00m });

            IRepository<Basket> baskets = new MockContext<Basket>();
            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2, BasketId = basket.Id });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 1, BasketId = basket.Id });

            baskets.Insert(basket);

            IBasketService basketService = new BasketService(products, baskets);

            IRepository<Order> orders = new MockContext<Order>();
            IOrderService orderService = new OrderService(orders);


            customers.Insert(new Customer { Id = "1", Email = "peritop@gmail.com", ZipCode = "60563" });

            IPrincipal fakeuser = new GenericPrincipal(new GenericIdentity("peritop@gmail.com" ,"Forms"), null);

            var controller = new BasketController(basketService, orderService, customers);
            var httpContext = new MockHttpContext();
            httpContext.User = fakeuser;
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket")
            {
                Value = basket.Id
            });

            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //Act
            Order order = new Order();
            controller.Checkout(order);

            //assert
            Assert.AreEqual(2, order.OrderItems.Count);
            Assert.AreEqual(0, basket.BasketItems.Count);

            Order orderInRep = orders.Find(order.Id);
            Assert.AreEqual(2, orderInRep.OrderItems.Count);

        }
    }
}
