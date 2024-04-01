using epizzahub.Entitites.Entitites;
using epizzahub.Models;
using epizzahub.Repositories.Interfaces;

namespace ePizzaHub.Repositories.Interfaces
{
    public interface ICartRepository: IRepostiory<Cart>
    {
        Cart GetCart(Guid CartId);
        CartModel GetCartDetails(Guid CartId);
        int DeleteItem(Guid CartId, int ItemId);
        int UpdateQuantity(Guid CartId, int ItemId, int Quantity);
        int UpdateCart(Guid CartId, int UserId);
    }
}
