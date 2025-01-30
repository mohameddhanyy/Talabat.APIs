using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        private readonly IGenericRepository<Order> _ordersRepo;

        public OrderService(IBasketRepository basketRepo,
            IGenericRepository<Product> productRepo,
            IGenericRepository<DeliveryMethod> deliveryMethodRepo,
            IGenericRepository<Order> ordersRepo)
        {
            _basketRepo = basketRepo;
            _productRepo = productRepo;
            _deliveryMethodRepo = deliveryMethodRepo;
            _ordersRepo = ordersRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address address)
        {
            // 1. Get Basket From Basket Repo
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // 2. Get Selected Items at Basket from Products Repo

            var orderItems = new List<OrderItem>();

            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _productRepo.GetAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(item.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                    orderItems.Add(orderItem);
                }
            }

            // 3. Calculate Subtotal
            var subTotal = orderItems.Sum(order => order.Quantity * order.Price);

            // 4. Get Delivery Method from DeliverMethod Repo
            var deliveryMethod = await _deliveryMethodRepo.GetAsync(deliveryMethodId);

            // 5. Create Order 
            var order = new Order(buyerEmail,address,deliveryMethod,orderItems,subTotal);
            await _ordersRepo.AddAsync(order);

            // 6. Save Changes

            }

        public Task<Order> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
        {
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
        }
    }
}
