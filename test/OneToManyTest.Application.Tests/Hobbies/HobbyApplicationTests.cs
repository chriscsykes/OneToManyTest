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
            result.Items.Any(x => x.Hobby.Id == Guid.Parse("6bc8cd04-9f8c-49a8-ab0a-27caf3470562")).ShouldBe(true);
            result.Items.Any(x => x.Hobby.Id == Guid.Parse("10895da3-80c8-4bd8-945c-c236793f8c19")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _hobbiesAppService.GetAsync(Guid.Parse("6bc8cd04-9f8c-49a8-ab0a-27caf3470562"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("6bc8cd04-9f8c-49a8-ab0a-27caf3470562"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new HobbyCreateDto
            {
                Name = "e007397760144a899f47f3a410dff4670084de641f054",
                YearsPerformed = 1691281096
            };

            // Act
            var serviceResult = await _hobbiesAppService.CreateAsync(input);

            // Assert
            var result = await _hobbyRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Name.ShouldBe("e007397760144a899f47f3a410dff4670084de641f054");
            result.YearsPerformed.ShouldBe(1691281096);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new HobbyUpdateDto()
            {
                Name = "f624789ad854496693163958fb29aabb60f63795d7974ea19b3f36b45ac296360f8cb8",
                YearsPerformed = 1085298784
            };

            // Act
            var serviceResult = await _hobbiesAppService.UpdateAsync(Guid.Parse("6bc8cd04-9f8c-49a8-ab0a-27caf3470562"), input);

            // Assert
            var result = await _hobbyRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Name.ShouldBe("f624789ad854496693163958fb29aabb60f63795d7974ea19b3f36b45ac296360f8cb8");
            result.YearsPerformed.ShouldBe(1085298784);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _hobbiesAppService.DeleteAsync(Guid.Parse("6bc8cd04-9f8c-49a8-ab0a-27caf3470562"));

            // Assert
            var result = await _hobbyRepository.FindAsync(c => c.Id == Guid.Parse("6bc8cd04-9f8c-49a8-ab0a-27caf3470562"));

            result.ShouldBeNull();
        }
    }
}