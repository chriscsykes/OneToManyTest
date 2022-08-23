using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using OneToManyTest.Shared;

namespace OneToManyTest.Hobbies
{
    public interface IHobbiesAppService : IApplicationService
    {
        Task<PagedResultDto<HobbyDto>> GetListAsync(GetHobbiesInput input);

        Task<HobbyDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<HobbyDto> CreateAsync(HobbyCreateDto input);

        Task<HobbyDto> UpdateAsync(Guid id, HobbyUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(HobbyExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}