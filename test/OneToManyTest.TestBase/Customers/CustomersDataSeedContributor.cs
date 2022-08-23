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
                id: Guid.Parse("55ada2cc-664d-4db8-99c4-848c41e3fb44"),
                firstName: "58cf764d83264053bd5fac18641e55",
                lastName: "09d1fea2",
                email: "ac357dfa4c6f414b9fcd90f520653c8530ef493af4fd4c9b",
                orderId: null
            ));

            await _customerRepository.InsertAsync(new Customer
            (
                id: Guid.Parse("690f27ce-e048-4f6d-966e-15bdc69ce5b0"),
                firstName: "9debb3788a254dd4be545c80a382fa5de93301741f49455b9a41b9d57ba2dc8f7aff51aedead424cb1",
                lastName: "c78e8fa6d7794ee696f40f057b2576966beeda35089f458584003a4cd139",
                email: "bc53a186a4d7416195c19efae93ec2e47180e7a06c7f44c",
                orderId: null
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}