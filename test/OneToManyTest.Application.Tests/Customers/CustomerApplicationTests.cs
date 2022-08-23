using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace OneToManyTest.Customers
{
    public class CustomersAppServiceTests : OneToManyTestApplicationTestBase
    {
        private readonly ICustomersAppService _customersAppService;
        private readonly IRepository<Customer, Guid> _customerRepository;

        public CustomersAppServiceTests()
        {
            _customersAppService = GetRequiredService<ICustomersAppService>();
            _customerRepository = GetRequiredService<IRepository<Customer, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _customersAppService.GetListAsync(new GetCustomersInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Customer.Id == Guid.Parse("4fa9098a-2453-498d-9d57-da3551ce969f")).ShouldBe(true);
            result.Items.Any(x => x.Customer.Id == Guid.Parse("753400d3-f59b-4771-b9da-0bcfee148116")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _customersAppService.GetAsync(Guid.Parse("4fa9098a-2453-498d-9d57-da3551ce969f"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("4fa9098a-2453-498d-9d57-da3551ce969f"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new CustomerCreateDto
            {
                FirstName = "cd92343430c34801957c1bec8dc34ad9616f0bb242e742a29d72ce6a11cd0098921dc69c50664f598b7ce254",
                LastName = "533fc502acab4e1f8cc6d2bbe8a",
                Email = "fdd726d2ce55472ab44b1eb2502a5aab221b18420ba",
                Address = "d998c8c677c54a6dbdef869c"
            };

            // Act
            var serviceResult = await _customersAppService.CreateAsync(input);

            // Assert
            var result = await _customerRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.FirstName.ShouldBe("cd92343430c34801957c1bec8dc34ad9616f0bb242e742a29d72ce6a11cd0098921dc69c50664f598b7ce254");
            result.LastName.ShouldBe("533fc502acab4e1f8cc6d2bbe8a");
            result.Email.ShouldBe("fdd726d2ce55472ab44b1eb2502a5aab221b18420ba");
            result.Address.ShouldBe("d998c8c677c54a6dbdef869c");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new CustomerUpdateDto()
            {
                FirstName = "8b10f0f2ae3f4747aae6919faf002e6fdfa88f8fe2ea45b0b94900",
                LastName = "e935890b7f2445a788af3724abc7204a842663ce89d946c382a",
                Email = "d32e4210d3f24088bbe2e817520a7589b94",
                Address = "5df05f324ffc42ea8c8ebad1760f59f3e20640da90c040e594af9f8a3c31d02ce90b08e6b9554ac98c1"
            };

            // Act
            var serviceResult = await _customersAppService.UpdateAsync(Guid.Parse("4fa9098a-2453-498d-9d57-da3551ce969f"), input);

            // Assert
            var result = await _customerRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.FirstName.ShouldBe("8b10f0f2ae3f4747aae6919faf002e6fdfa88f8fe2ea45b0b94900");
            result.LastName.ShouldBe("e935890b7f2445a788af3724abc7204a842663ce89d946c382a");
            result.Email.ShouldBe("d32e4210d3f24088bbe2e817520a7589b94");
            result.Address.ShouldBe("5df05f324ffc42ea8c8ebad1760f59f3e20640da90c040e594af9f8a3c31d02ce90b08e6b9554ac98c1");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _customersAppService.DeleteAsync(Guid.Parse("4fa9098a-2453-498d-9d57-da3551ce969f"));

            // Assert
            var result = await _customerRepository.FindAsync(c => c.Id == Guid.Parse("4fa9098a-2453-498d-9d57-da3551ce969f"));

            result.ShouldBeNull();
        }
    }
}