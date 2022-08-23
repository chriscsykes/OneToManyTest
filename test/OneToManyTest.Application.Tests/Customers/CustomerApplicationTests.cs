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
            result.Items.Any(x => x.Customer.Id == Guid.Parse("55ada2cc-664d-4db8-99c4-848c41e3fb44")).ShouldBe(true);
            result.Items.Any(x => x.Customer.Id == Guid.Parse("690f27ce-e048-4f6d-966e-15bdc69ce5b0")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _customersAppService.GetAsync(Guid.Parse("55ada2cc-664d-4db8-99c4-848c41e3fb44"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("55ada2cc-664d-4db8-99c4-848c41e3fb44"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new CustomerCreateDto
            {
                FirstName = "7f87a134a6c64decb10ab5cc7b9db8416dfc6dc5a55a",
                LastName = "1a26019b983846ad8b1e1d8d8c9e76eb020b13c14d3f4754a2b5a7bc4",
                Email = "4cf188fcf8db451aae9bce62215c98574d151cf7006443999ef272120b30"
            };

            // Act
            var serviceResult = await _customersAppService.CreateAsync(input);

            // Assert
            var result = await _customerRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.FirstName.ShouldBe("7f87a134a6c64decb10ab5cc7b9db8416dfc6dc5a55a");
            result.LastName.ShouldBe("1a26019b983846ad8b1e1d8d8c9e76eb020b13c14d3f4754a2b5a7bc4");
            result.Email.ShouldBe("4cf188fcf8db451aae9bce62215c98574d151cf7006443999ef272120b30");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new CustomerUpdateDto()
            {
                FirstName = "36ef3e2f3d164924af87f214f1f70d884e8d0a15d38b4cdca917c9f007e071c28a91450742eb4ef7ba",
                LastName = "6da33ae0b5364d",
                Email = "81d94ffc0fcb40e5ad1f3a9499b1e1363ee760225c274e4da1d0925ba4f4b18a"
            };

            // Act
            var serviceResult = await _customersAppService.UpdateAsync(Guid.Parse("55ada2cc-664d-4db8-99c4-848c41e3fb44"), input);

            // Assert
            var result = await _customerRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.FirstName.ShouldBe("36ef3e2f3d164924af87f214f1f70d884e8d0a15d38b4cdca917c9f007e071c28a91450742eb4ef7ba");
            result.LastName.ShouldBe("6da33ae0b5364d");
            result.Email.ShouldBe("81d94ffc0fcb40e5ad1f3a9499b1e1363ee760225c274e4da1d0925ba4f4b18a");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _customersAppService.DeleteAsync(Guid.Parse("55ada2cc-664d-4db8-99c4-848c41e3fb44"));

            // Assert
            var result = await _customerRepository.FindAsync(c => c.Id == Guid.Parse("55ada2cc-664d-4db8-99c4-848c41e3fb44"));

            result.ShouldBeNull();
        }
    }
}