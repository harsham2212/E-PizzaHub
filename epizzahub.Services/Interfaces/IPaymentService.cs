using epizzahub.Entitites.Entitites;
using Razorpay.Api;

namespace epizzahub.Services.Interfaces
{
    public interface IPaymentService : IService<PaymentDetail>
    {
        string CreateOrder(decimal amount, string currency, string receipt);
        Payment GetPaymentDetails(string paymentId);
        bool VerifySignature(string signature, string orderId, string paymentId);
        int SavePaymentDetails(PaymentDetail model);
    }
}
