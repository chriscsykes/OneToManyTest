using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace OneToManyTest.Hobbies
{
    public interface IHobbyRepository : IRepository<Hobby, Guid>
    {
        Task<HobbyWithNavigationProperties> GetWithNavigationPropertiesAsync(
    Guid id,
    CancellationToken cancellationToken = default
);

        Task<List<HobbyWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string filterText = null,
            string name = null,
            int? yearsPerformedMin = null,
            int? yearsPerformedMax = null,
            Guid? customerId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<Hobby>> GetListAsync(
                    string filterText = null,
                    string name = null,
                    int? yearsPerformedMin = null,
                    int? yearsPerformedMax = null,
                    string sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string filterText = null,
            string name = null,
            int? yearsPerformedMin = null,
            int? yearsPerformedMax = null,
            Guid? customerId = null,
            CancellationToken cancellationToken = default);
    }
}