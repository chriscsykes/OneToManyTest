using OneToManyTest.Orders;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using OneToManyTest.Customers;

namespace OneToManyTest.Customers
{
    public class CustomersDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly OrdersDataSeedContributor _ordersDataSeedContributor;

        public CustomersDataSeedContributor(ICustomerRepository customerRepository, IUnitOfWorkManager unitOfWorkManager, OrdersDataSeedContributor ordersDataSeedContributor)
        {
            _customerRepository = customerRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _ordersDataSeedContributor = ordersDataSeedContributor;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _ordersDataSeedContributor.SeedAsync(context);

            await _customerRepository.InsertAsync(new Customer
            (
                id: Guid.Parse("4fa9098a-2453-498d-9d57-da3551ce969f"),
                firstName: "4dfdda30d37d40c78250c3342f6409d9079e36ef4be54a438b7e08374841baa6f144cd9776134338a89fd",
                lastName: "4700abff8b6640a08844dbd74a88ed11edfbd7b94f0b4e018900e329f00c6738eddbee88f1c048d7b1c9e96204e2e",
                email: "12f79e7af51d4b728ba7ad4446437d69d17d57176a6f46b088b63d29aaf116def904e1fda1c2449e",
                address: "c771374af39041e9a401ab12dc881e099b41569008c445bfa2b99c0959f42f267db0e08d87c44cdf97214",
                orderId: null
            ));

            await _customerRepository.InsertAsync(new Customer
            (
                id: Guid.Parse("753400d3-f59b-4771-b9da-0bcfee148116"),
                firstName: "df5be33d2",
                lastName: "f29df78fcc6f49a98ecf937b6e9816ba6edd13b002bc481",
                email: "0151cd3965184a24a6b2c5b6442d26186cb62e6879514ccd8a6f56f09ffef08d89654b617a",
                address: "1e6450a3073e4b318901114832846fc27d37307afe76439a86605740cdadb51f1f9cc032df5d46539cc87b5cb66f90372",
                orderId: null
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}