using OneToManyTest.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace OneToManyTest.Hobbies
{
    public class HobbyManager : DomainService
    {
        private readonly IHobbyRepository _hobbyRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;

        public HobbyManager(IHobbyRepository hobbyRepository,
        IRepository<Customer, Guid> customerRepository)
        {
            _hobbyRepository = hobbyRepository;
            _customerRepository = customerRepository;
        }

        public async Task<Hobby> CreateAsync(
        List<Guid> customerIds,
        string name, int yearsPerformed)
        {
            var hobby = new Hobby(
             GuidGenerator.Create(),
             name, yearsPerformed
             );

            await SetCustomersAsync(hobby, customerIds);

            return await _hobbyRepository.InsertAsync(hobby);
        }

        public async Task<Hobby> UpdateAsync(
            Guid id,
            List<Guid> customerIds,
        string name, int yearsPerformed, [CanBeNull] string concurrencyStamp = null
        )
        {
            var queryable = await _hobbyRepository.WithDetailsAsync(x => x.Customers);
            var query = queryable.Where(x => x.Id == id);

            var hobby = await AsyncExecuter.FirstOrDefaultAsync(query);

            hobby.Name = name;
            hobby.YearsPerformed = yearsPerformed;

            await SetCustomersAsync(hobby, customerIds);

            hobby.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _hobbyRepository.UpdateAsync(hobby);
        }

        private async Task SetCustomersAsync(Hobby hobby, List<Guid> customerIds)
        {
            if (customerIds == null || !customerIds.Any())
            {
                hobby.RemoveAllCustomers();
                return;
            }

            var query = (await _customerRepository.GetQueryableAsync())
                .Where(x => customerIds.Contains(x.Id))
                .Select(x => x.Id);

            var customerIdsInDb = await AsyncExecuter.ToListAsync(query);
            if (!customerIdsInDb.Any())
            {
                return;
            }

            hobby.RemoveAllCustomersExceptGivenIds(customerIdsInDb);

            foreach (var customerId in customerIdsInDb)
            {
                hobby.AddCustomer(customerId);
            }
        }

    }
}