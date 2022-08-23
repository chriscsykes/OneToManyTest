using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace OneToManyTest.Hobbies
{
    public class HobbyManager : DomainService
    {
        private readonly IHobbyRepository _hobbyRepository;

        public HobbyManager(IHobbyRepository hobbyRepository)
        {
            _hobbyRepository = hobbyRepository;
        }

        public async Task<Hobby> CreateAsync(
        string name, int yearsPerformed)
        {
            var hobby = new Hobby(
             GuidGenerator.Create(),
             name, yearsPerformed
             );

            return await _hobbyRepository.InsertAsync(hobby);
        }

        public async Task<Hobby> UpdateAsync(
            Guid id,
            string name, int yearsPerformed, [CanBeNull] string concurrencyStamp = null
        )
        {
            var queryable = await _hobbyRepository.GetQueryableAsync();
            var query = queryable.Where(x => x.Id == id);

            var hobby = await AsyncExecuter.FirstOrDefaultAsync(query);

            hobby.Name = name;
            hobby.YearsPerformed = yearsPerformed;

            hobby.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _hobbyRepository.UpdateAsync(hobby);
        }

    }
}