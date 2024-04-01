using epizzahub.Entitites.Entitites;
using epizzahub.Models;
using epizzahub.Repositories.Interfaces;
using epizzahub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epizzahub.Services.Implementation
{
    public class OrderService : Service<Order>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository) : base(orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public OrderModel GetOrderDetails(string orderId)
        {
            return _orderRepository.GetOrderDetails(orderId);
        }

        public IEnumerable<Order> GetUserOrders(int userId)
        {
            return _orderRepository.GetUserOrders(userId);
        }

        public int PlaceOrder(int userId, string orderId, string paymentId, CartModel cart, AddressModel address)
        {
            Order order = new Order
            {
                Id = orderId,
                UserId = userId,
                PaymentId = paymentId,
                CreatedDate = DateTime.Now,
                Street = address.Street,
                City = address.City,
                Locality = address.Locality,
                ZipCode = address.ZipCode,
                PhoneNumber = address.PhoneNumber
            };
            foreach (var item in cart.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Total = item.Total
                });
            }
            _orderRepository.Add(order);
            return _orderRepository.SaveChanges();
        }
    }
}