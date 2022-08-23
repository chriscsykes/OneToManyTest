using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using OneToManyTest.EntityFrameworkCore;

namespace OneToManyTest.Orders
{
    public class EfCoreOrderRepository : EfCoreRepository<OneToManyTestDbContext, Order, Guid>, IOrderRepository
    {
        public EfCoreOrderRepository(IDbContextProvider<OneToManyTestDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<Order>> GetListAsync(
            string filterText = null,
            string item = null,
            int? quantityMin = null,
            int? quantityMax = null,
            decimal? priceMin = null,
            decimal? priceMax = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, item, quantityMin, quantityMax, priceMin, priceMax);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? OrderConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            string item = null,
            int? quantityMin = null,
            int? quantityMax = null,
            decimal? priceMin = null,
            decimal? priceMax = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, item, quantityMin, quantityMax, priceMin, priceMax);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Order> ApplyFilter(
            IQueryable<Order> query,
            string filterText,
            string item = null,
            int? quantityMin = null,
            int? quantityMax = null,
            decimal? priceMin = null,
            decimal? priceMax = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Item.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(item), e => e.Item.Contains(item))
                    .WhereIf(quantityMin.HasValue, e => e.Quantity >= quantityMin.Value)
                    .WhereIf(quantityMax.HasValue, e => e.Quantity <= quantityMax.Value)
                    .WhereIf(priceMin.HasValue, e => e.Price >= priceMin.Value)
                    .WhereIf(priceMax.HasValue, e => e.Price <= priceMax.Value);
        }
    }
}