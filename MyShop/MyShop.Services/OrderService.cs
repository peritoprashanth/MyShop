using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Services
{
    public class OrderService  : IOrderService
    {
        IRepository<Order> orderContext;
        public OrderService(IRepository<Order> OrderContext)
        {
            this.orderContext = orderContext;

        }
        
        public void CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItems)
        {
            foreach(var item in basketItems)
            {
                OrderItem o = new OrderItem();
                o.ProductId = item.Id;
                o.Image = item.Image;
                o.Price = item.Price;
                o.ProductName = item.ProductName;
                o.Quanity = item.Quantity;

                baseOrder.OrderItems.Add(o);
            }

            orderContext.Insert(baseOrder);
            orderContext.Commit();
        }
    }
}
