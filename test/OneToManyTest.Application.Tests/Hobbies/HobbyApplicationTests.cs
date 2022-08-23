using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace OneToManyTest.Hobbies
{
    public class HobbiesAppServiceTests : OneToManyTestApplicationTestBase
    {
        private readonly IHobbiesAppService _hobbiesAppService;
        private readonly IRepository<Hobby, Guid> _hobbyRepository;

        public HobbiesAppServiceTests()
        {
            _hobbiesAppService = GetRequiredService<IHobbiesAppService>();
            _hobbyRepository = GetRequiredService<IRepository<Hobby, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _hobbiesAppService.GetListAsync(new GetHobbiesInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("7d2d2416-a28f-46a8-a8ac-28c80cdc6b4f")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("7e619d44-a888-4d17-9478-582f85be0de7")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _hobbiesAppService.GetAsync(Guid.Parse("7d2d2416-a28f-46a8-a8ac-28c80cdc6b4f"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("7d2d2416-a28f-46a8-a8ac-28c80cdc6b4f"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new HobbyCreateDto
            {
                Name = "caad2297ff6c46949a226a07b036aacf86bc98d440aa43db986cb8ae65ac0f7e3c18e9adf13f47b78940099f48c",
                YearsPerformed = 816256787
            };

            // Act
            var serviceResult = await _hobbiesAppService.CreateAsync(input);

            // Assert
            var result = await _hobbyRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Name.ShouldBe("caad2297ff6c46949a226a07b036aacf86bc98d440aa43db986cb8ae65ac0f7e3c18e9adf13f47b78940099f48c");
            result.YearsPerformed.ShouldBe(816256787);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new HobbyUpdateDto()
            {
                Name = "1463e82da89f4ee6ab34e4e2d5273d4001b429e4c43e43f097ff400ab8df4415986e4c6bd1d44fbeb8f9539b9b51070f",
                YearsPerformed = 1868676416
            };

            // Act
            var serviceResult = await _hobbiesAppService.UpdateAsync(Guid.Parse("7d2d2416-a28f-46a8-a8ac-28c80cdc6b4f"), input);

            // Assert
            var result = await _hobbyRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Name.ShouldBe("1463e82da89f4ee6ab34e4e2d5273d4001b429e4c43e43f097ff400ab8df4415986e4c6bd1d44fbeb8f9539b9b51070f");
            result.YearsPerformed.ShouldBe(1868676416);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _hobbiesAppService.DeleteAsync(Guid.Parse("7d2d2416-a28f-46a8-a8ac-28c80cdc6b4f"));

            // Assert
            var result = await _hobbyRepository.FindAsync(c => c.Id == Guid.Parse("7d2d2416-a28f-46a8-a8ac-28c80cdc6b4f"));

            result.ShouldBeNull();
        }
    }
}