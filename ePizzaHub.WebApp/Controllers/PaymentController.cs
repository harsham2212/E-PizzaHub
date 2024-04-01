using epizzahub.Entitites.Entitites;
using epizzahub.Models;
using epizzahub.Services.Interfaces;
using ePizzaHub.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.WebApp.Controllers
{
    public class PaymentController : BaseController
    {
        IConfiguration _configuration;
        IPaymentService _paymentService;
        IOrderService _orderService;
        public PaymentController(IConfiguration configuration, IOrderService orderService, IPaymentService paymentService)
        {
            _configuration = configuration;
            _paymentService = paymentService;
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            PaymentModel payment = new PaymentModel();
            CartModel cart = TempData.Peek<CartModel>("Cart");
            if (cart != null)
            {
                payment.GrandTotal = cart.GrandTotal;
                payment.Currency = "INR";
                payment.Description = "Payment for Order";
                payment.Cart = cart;
                payment.Receipt = cart.Id.ToString();
                payment.RazorpayKey = _configuration["Razorpay:Key"];

                payment.OrderId = _paymentService.CreateOrder(payment.GrandTotal * 100, payment.Currency, payment.Receipt);
                return View(payment);
            }
            else
            {
                return RedirectToAction("Index", "Cart");
            }
        }

        [HttpPost]
        public IActionResult Status(IFormCollection form)
        {
            string paymentId = form["rzp_paymentid"];
            string orderId = form["rzp_orderid"];
            string signature = form["rzp_signature"];
            string transactionId = form["Receipt"];
            string currency = form["Currency"];

            var payment = _paymentService.GetPaymentDetails(paymentId);
            bool isValid = _paymentService.VerifySignature(signature, orderId, paymentId);
            if (isValid && payment != null)
            {
                PaymentDetail model = new PaymentDetail();
                CartModel cart = TempData.Peek<CartModel>("Cart");

                model.CartId = cart.Id;
                model.Total = cart.Total;
                model.Tax = cart.Tax;
                model.GrandTotal = cart.GrandTotal;
                model.CreatedDate = DateTime.Now;

                model.Status = payment.Attributes["status"]; //captured
                model.TransactionId = transactionId;
                model.Currency = payment.Attributes["currency"];
                model.Email = payment.Attributes["email"];
                model.Id = paymentId;
                model.UserId = CurrentUser.Id;

                int status = _paymentService.SavePaymentDetails(model);
                if (status > 0)
                {
                    Response.Cookies.Append("CId", "");
                    TempData.Remove("Cart");
                    AddressModel addressModel = TempData.Get<AddressModel>("Address");

                    _orderService.PlaceOrder(CurrentUser.Id, orderId, paymentId, cart, addressModel);
                    TempData.Set("PaymentDetails", model);
                    return RedirectToAction("Receipt");
                }
            }
            return View();
        }

        public IActionResult Receipt()
        {
            PaymentDetail model = TempData.Peek<PaymentDetail>("PaymentDetails");
            return View(model);
        }
    }
}