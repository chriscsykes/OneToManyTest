using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace OneToManyTest.Orders
{
    public class OrderManager : DomainService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> CreateAsync(
        string item, int quantity, decimal price)
        {
            var order = new Order(
             GuidGenerator.Create(),
             item, quantity, price
             );

            return await _orderRepository.InsertAsync(order);
        }

        public async Task<Order> UpdateAsync(
            Guid id,
            string item, int quantity, decimal price, [CanBeNull] string concurrencyStamp = null
        )
        {
            var queryable = await _orderRepository.GetQueryableAsync();
            var query = queryable.Where(x => x.Id == id);

            var order = await AsyncExecuter.FirstOrDefaultAsync(query);

            order.Item = item;
            order.Quantity = quantity;
            order.Price = price;

            order.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _orderRepository.UpdateAsync(order);
        }

    }
}