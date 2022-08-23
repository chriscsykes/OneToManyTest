using OneToManyTest.Customers;
using OneToManyTest.Customers;
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

namespace OneToManyTest.Hobbies
{
    public class EfCoreHobbyRepository : EfCoreRepository<OneToManyTestDbContext, Hobby, Guid>, IHobbyRepository
    {
        public EfCoreHobbyRepository(IDbContextProvider<OneToManyTestDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<HobbyWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id).Include(x => x.Customers)
                .Select(hobby => new HobbyWithNavigationProperties
                {
                    Hobby = hobby,
                    Customers = (from hobbyCustomers in hobby.Customers
                                 join _customer in dbContext.Set<Customer>() on hobbyCustomers.CustomerId equals _customer.Id
                                 select _customer).ToList()
                }).FirstOrDefault();
        }

        public async Task<List<HobbyWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string filterText = null,
            string name = null,
            int? yearsPerformedMin = null,
            int? yearsPerformedMax = null,
            Guid? customerId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, name, yearsPerformedMin, yearsPerformedMax, customerId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? HobbyConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<HobbyWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from hobby in (await GetDbSetAsync())

                   select new HobbyWithNavigationProperties
                   {
                       Hobby = hobby,
                       Customers = new List<Customer>()
                   };
        }

        protected virtual IQueryable<HobbyWithNavigationProperties> ApplyFilter(
            IQueryable<HobbyWithNavigationProperties> query,
            string filterText,
            string name = null,
            int? yearsPerformedMin = null,
            int? yearsPerformedMax = null,
            Guid? customerId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Hobby.Name.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Hobby.Name.Contains(name))
                    .WhereIf(yearsPerformedMin.HasValue, e => e.Hobby.YearsPerformed >= yearsPerformedMin.Value)
                    .WhereIf(yearsPerformedMax.HasValue, e => e.Hobby.YearsPerformed <= yearsPerformedMax.Value)
                    .WhereIf(customerId != null && customerId != Guid.Empty, e => e.Hobby.Customers.Any(x => x.CustomerId == customerId));
        }

        public async Task<List<Hobby>> GetListAsync(
            string filterText = null,
            string name = null,
            int? yearsPerformedMin = null,
            int? yearsPerformedMax = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, name, yearsPerformedMin, yearsPerformedMax);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? HobbyConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            string name = null,
            int? yearsPerformedMin = null,
            int? yearsPerformedMax = null,
            Guid? customerId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, name, yearsPerformedMin, yearsPerformedMax, customerId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Hobby> ApplyFilter(
            IQueryable<Hobby> query,
            string filterText,
            string name = null,
            int? yearsPerformedMin = null,
            int? yearsPerformedMax = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Name.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Name.Contains(name))
                    .WhereIf(yearsPerformedMin.HasValue, e => e.YearsPerformed >= yearsPerformedMin.Value)
                    .WhereIf(yearsPerformedMax.HasValue, e => e.YearsPerformed <= yearsPerformedMax.Value);
        }
    }
}