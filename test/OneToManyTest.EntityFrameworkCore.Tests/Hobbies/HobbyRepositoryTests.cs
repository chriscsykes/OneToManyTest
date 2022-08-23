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
                    name: "085570fe9eb64026a67706624afca7e5ec77be6301ac4ae9b173089b3ba1c1b12b852babe4814ef1b1098fa43"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("7d2d2416-a28f-46a8-a8ac-28c80cdc6b4f"));
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
                    name: "4fcd00be811041919dca6e5d21d8c3c0216715ba3c464bb599065a5e91b256dbfc9cdcad310941"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}