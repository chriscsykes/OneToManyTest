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
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, name, yearsPerformedMin, yearsPerformedMax);
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