using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using OneToManyTest.Hobbies;
using OneToManyTest.EntityFrameworkCore;
using Xunit;

namespace OneToManyTest.Hobbies
{
    public class HobbyRepositoryTests : OneToManyTestEntityFrameworkCoreTestBase
    {
        private readonly IHobbyRepository _hobbyRepository;

        public HobbyRepositoryTests()
        {
            _hobbyRepository = GetRequiredService<IHobbyRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _hobbyRepository.GetListAsync(
                    name: "609d3f9e8ad441de9413d94d4e923a475fba0eaba70b4251aae2cf4d0ad61fd5d998028e665445fbb8417342ed0c"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("6bc8cd04-9f8c-49a8-ab0a-27caf3470562"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _hobbyRepository.GetCountAsync(
                    name: "fa31da7a653"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}