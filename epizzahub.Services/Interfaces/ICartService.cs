using epizzahub.Entitites.Entitites;
using epizzahub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epizzahub.Services.Interfaces
{
    public interface ICartService : IService<Cart>
    {
        int GetCartCount(Guid CartId);
        CartModel GetCartDetails(Guid CartId);
        Cart AddItem(int UserId, Guid CartId, int ItemId, decimal UnitPrice, int Quantity);
        int DeleteItem(Guid CartId, int ItemId);
        int UpdateQuantity(Guid CartId, int ItemId, int Quantity);
        int UpdateCart(Guid CartId, int UserId);
    }
}
