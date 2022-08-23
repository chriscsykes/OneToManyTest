using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using OneToManyTest.Customers;
using OneToManyTest.EntityFrameworkCore;
using Xunit;

namespace OneToManyTest.Customers
{
    public class CustomerRepositoryTests : OneToManyTestEntityFrameworkCoreTestBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerRepositoryTests()
        {
            _customerRepository = GetRequiredService<ICustomerRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _customerRepository.GetListAsync(
                    firstName: "58cf764d83264053bd5fac18641e55",
                    lastName: "09d1fea2",
                    email: "ac357dfa4c6f414b9fcd90f520653c8530ef493af4fd4c9b"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("55ada2cc-664d-4db8-99c4-848c41e3fb44"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _customerRepository.GetCountAsync(
                    firstName: "9debb3788a254dd4be545c80a382fa5de93301741f49455b9a41b9d57ba2dc8f7aff51aedead424cb1",
                    lastName: "c78e8fa6d7794ee696f40f057b2576966beeda35089f458584003a4cd139",
                    email: "bc53a186a4d7416195c19efae93ec2e47180e7a06c7f44c"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}