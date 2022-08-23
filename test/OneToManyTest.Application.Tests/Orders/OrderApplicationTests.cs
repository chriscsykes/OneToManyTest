using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace OneToManyTest.Orders
{
    public class OrdersAppServiceTests : OneToManyTestApplicationTestBase
    {
        private readonly IOrdersAppService _ordersAppService;
        private readonly IRepository<Order, Guid> _orderRepository;

        public OrdersAppServiceTests()
        {
            _ordersAppService = GetRequiredService<IOrdersAppService>();
            _orderRepository = GetRequiredService<IRepository<Order, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _ordersAppService.GetListAsync(new GetOrdersInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("506a33da-2837-4335-90e0-e6f0289ffbd5")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("f7b590c8-a013-4ef9-ad83-05dd2949771d")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _ordersAppService.GetAsync(Guid.Parse("506a33da-2837-4335-90e0-e6f0289ffbd5"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("506a33da-2837-4335-90e0-e6f0289ffbd5"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new OrderCreateDto
            {
                Item = "c4f057bfe28047a6a41012c31725a4d8c4fb66807abf488383c587d5c",
                Quantity = 818899799,
                Price = 701899487
            };

            // Act
            var serviceResult = await _ordersAppService.CreateAsync(input);

            // Assert
            var result = await _orderRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Item.ShouldBe("c4f057bfe28047a6a41012c31725a4d8c4fb66807abf488383c587d5c");
            result.Quantity.ShouldBe(818899799);
            result.Price.ShouldBe(701899487);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new OrderUpdateDto()
            {
                Item = "e9543a7b768c46b9809d783eb86fbd06bba576058e6d47b18286c7700b8ae",
                Quantity = 413572875,
                Price = 1474174650
            };

            // Act
            var serviceResult = await _ordersAppService.UpdateAsync(Guid.Parse("506a33da-2837-4335-90e0-e6f0289ffbd5"), input);

            // Assert
            var result = await _orderRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Item.ShouldBe("e9543a7b768c46b9809d783eb86fbd06bba576058e6d47b18286c7700b8ae");
            result.Quantity.ShouldBe(413572875);
            result.Price.ShouldBe(1474174650);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _ordersAppService.DeleteAsync(Guid.Parse("506a33da-2837-4335-90e0-e6f0289ffbd5"));

            // Assert
            var result = await _orderRepository.FindAsync(c => c.Id == Guid.Parse("506a33da-2837-4335-90e0-e6f0289ffbd5"));

            result.ShouldBeNull();
        }
    }
}