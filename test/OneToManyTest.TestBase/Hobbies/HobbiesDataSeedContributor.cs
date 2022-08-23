using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using OneToManyTest.Hobbies;

namespace OneToManyTest.Hobbies
{
    public class HobbiesDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IHobbyRepository _hobbyRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public HobbiesDataSeedContributor(IHobbyRepository hobbyRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _hobbyRepository = hobbyRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _hobbyRepository.InsertAsync(new Hobby
            (
                id: Guid.Parse("6bc8cd04-9f8c-49a8-ab0a-27caf3470562"),
                name: "609d3f9e8ad441de9413d94d4e923a475fba0eaba70b4251aae2cf4d0ad61fd5d998028e665445fbb8417342ed0c",
                yearsPerformed: 225927928
            ));

            await _hobbyRepository.InsertAsync(new Hobby
            (
                id: Guid.Parse("10895da3-80c8-4bd8-945c-c236793f8c19"),
                name: "fa31da7a653",
                yearsPerformed: 1576983372
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}