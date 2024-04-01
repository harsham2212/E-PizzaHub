using epizzahub.Entitites.Entitites;
using epizzahub.Models;
using epizzahub.Repositories.Implemenations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epizzahub.Repositories.Interfaces
{
    public interface IOrderRepository : IRepostiory<Order>
    {
        OrderModel GetOrderDetails(string orderId);
        IEnumerable<Order> GetUserOrders(int userId);
    }
}
