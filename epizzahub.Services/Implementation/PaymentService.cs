using epizzahub.Entitites.Entitites;
using epizzahub.Repositories.Interfaces;
using epizzahub.Services.Interfaces;
using ePizzaHub.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace epizzahub.Services.Implementation
{
    public class PaymentService : Service<PaymentDetail>, IPaymentService
    {
        IRepostiory<PaymentDetail> _paymentRepo;
        IConfiguration _configuration;
        ICartRepository _cartRepo;
        private RazorpayClient _razorpayClient;
        public PaymentService(IRepostiory<PaymentDetail> paymentRepo, IConfiguration configuration, ICartRepository cartRepo) : base(paymentRepo)
        {
            _paymentRepo = paymentRepo;
            _configuration = configuration;
            _cartRepo = cartRepo;
            _razorpayClient = new RazorpayClient(_configuration["Razorpay:Key"], _configuration["Razorpay:Secret"]);
        }

        public string CreateOrder(decimal amount, string currency, string receipt)
        {
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", amount); // amount in the smallest currency unit
            options.Add("receipt", receipt);
            options.Add("currency", currency);
            Razorpay.Api.Order order = _razorpayClient.Order.Create(options);
            return order["id"].ToString();
        }

        public Payment GetPaymentDetails(string paymentId)
        {
            return _razorpayClient.Payment.Fetch(paymentId);
        }

        public int SavePaymentDetails(PaymentDetail model)
        {
            _paymentRepo.Add(model);
            Cart cart = _cartRepo.GetCart(model.CartId);
            cart.IsActive = false;
            return _paymentRepo.SaveChanges();
        }

        public bool VerifySignature(string signature, string orderId, string paymentId)
        {
            string payload = string.Format("{0}|{1}", orderId, paymentId);
            string secret = RazorpayClient.Secret;
            string actualSignature = getActualSignature(payload, secret);
            return actualSignature.Equals(signature);
        }

        private static string getActualSignature(string payload, string secret)
        {
            byte[] secretBytes = StringEncode(secret);
            HMACSHA256 hashHmac = new HMACSHA256(secretBytes);
            var bytes = StringEncode(payload);

            return HashEncode(hashHmac.ComputeHash(bytes));
        }

        private static byte[] StringEncode(string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }

        private static string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
