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
                id: Guid.Parse("7d2d2416-a28f-46a8-a8ac-28c80cdc6b4f"),
                name: "085570fe9eb64026a67706624afca7e5ec77be6301ac4ae9b173089b3ba1c1b12b852babe4814ef1b1098fa43",
                yearsPerformed: 1150191496
            ));

            await _hobbyRepository.InsertAsync(new Hobby
            (
                id: Guid.Parse("7e619d44-a888-4d17-9478-582f85be0de7"),
                name: "4fcd00be811041919dca6e5d21d8c3c0216715ba3c464bb599065a5e91b256dbfc9cdcad310941",
                yearsPerformed: 1548517395
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}