using epizzahub.Entitites.Entitites;
using epizzahub.Models;
using ePizzaHub.Repositories.Interfaces;
using epizzahub.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using epizzahub.Repositories.Interfaces;

namespace epizzahub.Services.Implementation
{
    public class CartService : Service<Cart>, ICartService
    {
        ICartRepository _cartRepo;
        IConfiguration _config;
        IRepostiory<CartItem> _cartItemRepo;
        public CartService(ICartRepository cartRepo, IConfiguration config, IRepostiory<CartItem> cartItemRepo) : base(cartRepo)
        {
            _cartRepo = cartRepo;
            _config = config;
            _cartItemRepo = cartItemRepo;
        }

        public Cart AddItem(int UserId, Guid CartId, int ItemId, decimal UnitPrice, int Quantity)
        {
            try
            {
                Cart cart = _cartRepo.GetCart(CartId);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = UserId,
                        Id = CartId,
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    };
                    CartItem cartItem = new CartItem
                    {
                        ItemId = ItemId,
                        Quantity = Quantity,
                        UnitPrice = UnitPrice,
                        CartId = CartId
                    };

                    cart.CartItems.Add(cartItem);
                    _cartRepo.Add(cart);
                    _cartRepo.SaveChanges();
                }
                else
                {
                    CartItem cartItem = cart.CartItems.Where(x => x.ItemId == ItemId).FirstOrDefault();
                    if (cartItem != null)
                    {
                        cartItem.Quantity += Quantity;
                        _cartItemRepo.Update(cartItem);
                        _cartItemRepo.SaveChanges();
                    }
                    else
                    {
                        cartItem = new CartItem
                        {
                            ItemId = ItemId,
                            Quantity = Quantity,
                            UnitPrice = UnitPrice,
                            CartId = CartId
                        };
                        cart.CartItems.Add(cartItem);

                        _cartItemRepo.Update(cartItem);
                        _cartItemRepo.SaveChanges();
                    }
                }
                return cart;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int DeleteItem(Guid CartId, int ItemId)
        {
            return _cartRepo.DeleteItem(CartId, ItemId);
        }

        public int GetCartCount(Guid CartId)
        {
            var cart = _cartRepo.GetCart(CartId);
            if (cart != null)
            {
                return cart.CartItems.Count;
            }
            return 0;
        }

        public CartModel GetCartDetails(Guid CartId)
        {
            var model = _cartRepo.GetCartDetails(CartId);
            if (model != null && model.Items.Count > 0)
            {
                decimal subtotal = 0;
                foreach (var item in model.Items)
                {
                    item.Total = item.UnitPrice * item.Quantity;
                    subtotal += item.Total;
                }
                model.Total = subtotal;
                model.Tax = Math.Round((model.Total * Convert.ToDecimal(_config["Tax:GST"])) / 100, 2);
                model.GrandTotal = model.Total + model.Tax;
            }
            return model;
        }

        public int UpdateCart(Guid CartId, int UserId)
        {
            return _cartRepo.UpdateCart(CartId, UserId);
        }

        public int UpdateQuantity(Guid CartId, int ItemId, int Quantity)
        {
            return _cartRepo.UpdateQuantity(CartId, ItemId, Quantity);
        }
    }
}