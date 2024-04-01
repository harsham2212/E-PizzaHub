using epizzahub.Entitites.Entitites;
using epizzahub.Entitites;
using epizzahub.Models;
using epizzahub.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace epizzahub.Repositories.Implemenations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDBContext db) : base(db)
        {

        }
        public OrderModel GetOrderDetails(string orderId)
        {
            var model = (from order in db.Orders
                         join payment in db.PaymentDetails on order.PaymentId equals payment.Id
                         where order.Id == orderId
                         select new OrderModel
                         {
                             Id = order.Id,
                             UserId = order.UserId,
                             Total = payment.Total,
                             Tax = payment.Tax,
                             GrandTotal = payment.GrandTotal,
                             CreatedDate = order.CreatedDate,
                             Items = (from orderItem in db.OrderItems
                                      join item in db.Items on orderItem.ItemId equals item.Id
                                      where orderItem.OrderId == orderId
                                      select new ItemModel
                                      {
                                          Id = orderItem.Id,
                                          ItemId = orderItem.ItemId,
                                          UnitPrice = orderItem.UnitPrice,
                                          Quantity = orderItem.Quantity,
                                          Name = item.Name,
                                          Description = item.Description,
                                          ImageUrl = item.ImageUrl
                                      }).ToList()
                         }).FirstOrDefault();
            return model;
        }

        public IEnumerable<Order> GetUserOrders(int userId)
        {
            return db.Orders.Include(o => o.OrderItems).Where(o => o.UserId == userId).ToList();
        }
    }
}
