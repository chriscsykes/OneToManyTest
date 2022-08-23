using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using OneToManyTest.Orders;
using OneToManyTest.EntityFrameworkCore;
using Xunit;

namespace OneToManyTest.Orders
{
    public class OrderRepositoryTests : OneToManyTestEntityFrameworkCoreTestBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderRepositoryTests()
        {
            _orderRepository = GetRequiredService<IOrderRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _orderRepository.GetListAsync(
                    item: "5331e2db0e064ebb8aa491e4a79"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("506a33da-2837-4335-90e0-e6f0289ffbd5"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _orderRepository.GetCountAsync(
                    item: "efb28e3aefea4138a23cb8184450dd8fa2946646260f4fd380d8b889a9f446c454697899bee74ca597e1"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}