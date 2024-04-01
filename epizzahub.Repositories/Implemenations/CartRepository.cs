using epizzahub.Entitites;
using epizzahub.Entitites.Entitites;
using epizzahub.Models;
using epizzahub.Repositories.Implemenations;
using ePizzaHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace ePizzaHub.Repositories.Implementations
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(AppDBContext db) : base(db)
        {

        }

        public int DeleteItem(Guid CartId, int ItemId)
        {
            var item = db.CartItems.Where(c => c.CartId == CartId && c.ItemId == ItemId).FirstOrDefault();
            if (item != null)
            {
                db.CartItems.Remove(item);
                return db.SaveChanges();
            }
            return 0;
        }

        public Cart GetCart(Guid CartId)
        {
            return db.Carts.Include(c => c.CartItems).Where(c => c.Id == CartId && c.IsActive).FirstOrDefault();
        }

        public CartModel GetCartDetails(Guid CartId)
        {
            var model = (from cart in db.Carts
                         where cart.Id == CartId && cart.IsActive == true
                         select new CartModel
                         {
                             Id = cart.Id,
                             UserId = cart.UserId,
                             CreatedDate = cart.CreatedDate,
                             Items = (from cartItem in db.CartItems
                                      join item in db.Items on cartItem.ItemId equals item.Id
                                      where cartItem.CartId == CartId
                                      select new ItemModel
                                      {
                                          Id = cartItem.Id,
                                          ItemId = cartItem.ItemId,
                                          Name = item.Name,
                                          UnitPrice = cartItem.UnitPrice,
                                          Quantity = cartItem.Quantity,
                                          Description = item.Description,
                                          ImageUrl = item.ImageUrl
                                      }).ToList()
                         }).FirstOrDefault();
            return model;
        }

        public int UpdateCart(Guid CartId, int UserId)
        {
            Cart cart = db.Carts.Where(c => c.Id == CartId && c.IsActive).FirstOrDefault();
            if (cart != null)
            {
                cart.UserId = UserId;
                return db.SaveChanges();
            }
            return 0;
        }

        public int UpdateQuantity(Guid CartId, int Id, int Quantity)
        {
            bool flag = false;
            var cart = db.Carts.Include(c => c.CartItems).Where(c => c.Id == CartId && c.IsActive).FirstOrDefault();
            if (cart != null)
            {
                var cartItems = cart.CartItems.ToList();
                for (int i = 0; i < cartItems.Count; i++)
                {
                    if (cartItems[i].Id == Id)
                    {
                        cartItems[i].Quantity += Quantity;
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    cart.CartItems = cartItems;
                    return db.SaveChanges();
                }
            }
            return 0;
        }
    }
}
