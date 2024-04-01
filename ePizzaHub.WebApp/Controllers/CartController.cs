using epizzahub.Entitites.Entitites;
using epizzahub.Models;
using epizzahub.Services.Interfaces;
using ePizzaHub.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.WebApp.Controllers
{
    public class CartController : BaseController
    {
        ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        Guid CartId
        {
            get
            {
                Guid Id;
                if (Request.Cookies["CId"] == null)
                {
                    Id = Guid.NewGuid();
                    Response.Cookies.Append("CId", Id.ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(1) });
                }
                else
                {
                    Id = Guid.Parse(Request.Cookies["CId"]);
                }
                return Id;
            }
        }


        public IActionResult Index()
        {
            CartModel cart = _cartService.GetCartDetails(CartId);
            return View(cart);
        }


        [Route("/Cart/UpdateQuantity/{id}/{quantity}")]
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var count = _cartService.UpdateQuantity(CartId, id, quantity);
            return Json(count);
        }


        [Route("/Cart/AddToCart/{ItemId}/{UnitPrice}/{Quantity}")]
        public IActionResult AddToCart(int ItemId, decimal UnitPrice, int Quantity)
        {
            int userId = CurrentUser != null ? CurrentUser.Id : 0;
            if (ItemId > 0 && UnitPrice > 0 && Quantity > 0)
            {
                Cart cart = _cartService.AddItem(userId, CartId, ItemId, UnitPrice, Quantity);
                if (cart != null)
                {
                    return Json(new { status = "success", count = cart.CartItems.Count });
                }
            }
            return Json(new { status = "failed", count = 0 });
        }

        public IActionResult DeleteItem(int id)
        {
            var count = _cartService.DeleteItem(CartId, id);
            return Json(count);
        }

        public JsonResult GetCartCount()
        {
            int count = _cartService.GetCartCount(CartId);
            return Json(count);
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(AddressModel model)
        {
            if (ModelState.IsValid)
            {
                CartModel cart = _cartService.GetCartDetails(CartId);
                if (cart != null)
                {
                    _cartService.UpdateCart(CartId, CurrentUser.Id);
                }
                TempData.Set("Address", model);
                TempData.Set("Cart", cart);
                return RedirectToAction("Index", "Payment");
            }
            return View();
        }
    }
}
