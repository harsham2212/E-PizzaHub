using epizzahub.Entitites.Entitites;
using epizzahub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epizzahub.Services.Interfaces
{
    public interface IOrderService : IService<Order>
    {
        OrderModel GetOrderDetails(string orderId);
        IEnumerable<Order> GetUserOrders(int userId);

        int PlaceOrder(int userId, string orderId, string paymentId, CartModel cart, AddressModel address);
    }
}