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

namespace OneToManyTest.Customers
{
    public class EfCoreCustomerRepository : EfCoreRepository<OneToManyTestDbContext, Customer, Guid>, ICustomerRepository
    {
        public EfCoreCustomerRepository(IDbContextProvider<OneToManyTestDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<CustomerWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(customer => new CustomerWithNavigationProperties
                {
                    Customer = customer,
                    Order = dbContext.Orders.FirstOrDefault(c => c.Id == customer.OrderId)
                }).FirstOrDefault();
        }

        public async Task<List<CustomerWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string filterText = null,
            string firstName = null,
            string lastName = null,
            string email = null,
            Guid? orderId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, firstName, lastName, email, orderId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CustomerConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<CustomerWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from customer in (await GetDbSetAsync())
                   join order in (await GetDbContextAsync()).Orders on customer.OrderId equals order.Id into orders
                   from order in orders.DefaultIfEmpty()

                   select new CustomerWithNavigationProperties
                   {
                       Customer = customer,
                       Order = order
                   };
        }

        protected virtual IQueryable<CustomerWithNavigationProperties> ApplyFilter(
            IQueryable<CustomerWithNavigationProperties> query,
            string filterText,
            string firstName = null,
            string lastName = null,
            string email = null,
            Guid? orderId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Customer.FirstName.Contains(filterText) || e.Customer.LastName.Contains(filterText) || e.Customer.Email.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(firstName), e => e.Customer.FirstName.Contains(firstName))
                    .WhereIf(!string.IsNullOrWhiteSpace(lastName), e => e.Customer.LastName.Contains(lastName))
                    .WhereIf(!string.IsNullOrWhiteSpace(email), e => e.Customer.Email.Contains(email))
                    .WhereIf(orderId != null && orderId != Guid.Empty, e => e.Order != null && e.Order.Id == orderId);
        }

        public async Task<List<Customer>> GetListAsync(
            string filterText = null,
            string firstName = null,
            string lastName = null,
            string email = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, firstName, lastName, email);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CustomerConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            string firstName = null,
            string lastName = null,
            string email = null,
            Guid? orderId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, firstName, lastName, email, orderId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Customer> ApplyFilter(
            IQueryable<Customer> query,
            string filterText,
            string firstName = null,
            string lastName = null,
            string email = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.FirstName.Contains(filterText) || e.LastName.Contains(filterText) || e.Email.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(firstName), e => e.FirstName.Contains(firstName))
                    .WhereIf(!string.IsNullOrWhiteSpace(lastName), e => e.LastName.Contains(lastName))
                    .WhereIf(!string.IsNullOrWhiteSpace(email), e => e.Email.Contains(email));
        }
    }
}