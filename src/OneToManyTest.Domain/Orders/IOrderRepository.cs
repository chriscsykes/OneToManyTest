using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace OneToManyTest.Orders
{
    public interface IOrderRepository : IRepository<Order, Guid>
    {
        Task<List<Order>> GetListAsync(
            string filterText = null,
            string item = null,
            int? quantityMin = null,
            int? quantityMax = null,
            decimal? priceMin = null,
            decimal? priceMax = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string filterText = null,
            string item = null,
            int? quantityMin = null,
            int? quantityMax = null,
            decimal? priceMin = null,
            decimal? priceMax = null,
            CancellationToken cancellationToken = default);
    }
}