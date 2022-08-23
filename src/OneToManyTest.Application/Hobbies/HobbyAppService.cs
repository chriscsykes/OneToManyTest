using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using OneToManyTest.Permissions;
using OneToManyTest.Hobbies;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using OneToManyTest.Shared;

namespace OneToManyTest.Hobbies
{

    [Authorize(OneToManyTestPermissions.Hobbies.Default)]
    public class HobbiesAppService : ApplicationService, IHobbiesAppService
    {
        private readonly IDistributedCache<HobbyExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IHobbyRepository _hobbyRepository;
        private readonly HobbyManager _hobbyManager;

        public HobbiesAppService(IHobbyRepository hobbyRepository, HobbyManager hobbyManager, IDistributedCache<HobbyExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _hobbyRepository = hobbyRepository;
            _hobbyManager = hobbyManager;
        }

        public virtual async Task<PagedResultDto<HobbyDto>> GetListAsync(GetHobbiesInput input)
        {
            var totalCount = await _hobbyRepository.GetCountAsync(input.FilterText, input.Name, input.YearsPerformedMin, input.YearsPerformedMax);
            var items = await _hobbyRepository.GetListAsync(input.FilterText, input.Name, input.YearsPerformedMin, input.YearsPerformedMax, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<HobbyDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Hobby>, List<HobbyDto>>(items)
            };
        }

        public virtual async Task<HobbyDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Hobby, HobbyDto>(await _hobbyRepository.GetAsync(id));
        }

        [Authorize(OneToManyTestPermissions.Hobbies.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _hobbyRepository.DeleteAsync(id);
        }

        [Authorize(OneToManyTestPermissions.Hobbies.Create)]
        public virtual async Task<HobbyDto> CreateAsync(HobbyCreateDto input)
        {

            var hobby = await _hobbyManager.CreateAsync(
            input.Name, input.YearsPerformed
            );

            return ObjectMapper.Map<Hobby, HobbyDto>(hobby);
        }

        [Authorize(OneToManyTestPermissions.Hobbies.Edit)]
        public virtual async Task<HobbyDto> UpdateAsync(Guid id, HobbyUpdateDto input)
        {

            var hobby = await _hobbyManager.UpdateAsync(
            id,
            input.Name, input.YearsPerformed, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Hobby, HobbyDto>(hobby);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(HobbyExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _hobbyRepository.GetListAsync(input.FilterText, input.Name, input.YearsPerformedMin, input.YearsPerformedMax);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Hobby>, List<HobbyExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Hobbies.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new HobbyExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}