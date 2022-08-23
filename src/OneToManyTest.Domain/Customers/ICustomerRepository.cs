using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace OneToManyTest.Customers
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
        Task<CustomerWithNavigationProperties> GetWithNavigationPropertiesAsync(
    Guid id,
    CancellationToken cancellationToken = default
);

        Task<List<CustomerWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string filterText = null,
            string firstName = null,
            string lastName = null,
            string email = null,
            Guid? orderId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<Customer>> GetListAsync(
                    string filterText = null,
                    string firstName = null,
                    string lastName = null,
                    string email = null,
                    string sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string filterText = null,
            string firstName = null,
            string lastName = null,
            string email = null,
            Guid? orderId = null,
            CancellationToken cancellationToken = default);
    }
}