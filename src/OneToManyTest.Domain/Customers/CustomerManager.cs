using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace OneToManyTest.Customers
{
    public class CustomerManager : DomainService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerManager(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> CreateAsync(
        Guid? orderId, string firstName, string lastName, string email, string address)
        {
            var customer = new Customer(
             GuidGenerator.Create(),
             orderId, firstName, lastName, email, address
             );

            return await _customerRepository.InsertAsync(customer);
        }

        public async Task<Customer> UpdateAsync(
            Guid id,
            Guid? orderId, string firstName, string lastName, string email, string address, [CanBeNull] string concurrencyStamp = null
        )
        {
            var queryable = await _customerRepository.GetQueryableAsync();
            var query = queryable.Where(x => x.Id == id);

            var customer = await AsyncExecuter.FirstOrDefaultAsync(query);

            customer.OrderId = orderId;
            customer.FirstName = firstName;
            customer.LastName = lastName;
            customer.Email = email;
            customer.Address = address;

            customer.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _customerRepository.UpdateAsync(customer);
        }

    }
}