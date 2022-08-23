using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using OneToManyTest.Orders;

namespace OneToManyTest.Orders
{
    public class OrdersDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public OrdersDataSeedContributor(IOrderRepository orderRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _orderRepository = orderRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _orderRepository.InsertAsync(new Order
            (
                id: Guid.Parse("506a33da-2837-4335-90e0-e6f0289ffbd5"),
                item: "5331e2db0e064ebb8aa491e4a79",
                quantity: 1864904864,
                price: 432196996
            ));

            await _orderRepository.InsertAsync(new Order
            (
                id: Guid.Parse("f7b590c8-a013-4ef9-ad83-05dd2949771d"),
                item: "efb28e3aefea4138a23cb8184450dd8fa2946646260f4fd380d8b889a9f446c454697899bee74ca597e1",
                quantity: 1062278021,
                price: 1249998798
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}