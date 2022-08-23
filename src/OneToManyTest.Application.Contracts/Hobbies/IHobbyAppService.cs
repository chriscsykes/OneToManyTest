using OneToManyTest.Shared;
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
        Task<PagedResultDto<HobbyWithNavigationPropertiesDto>> GetListAsync(GetHobbiesInput input);

        Task<HobbyWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<HobbyDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetCustomerLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<HobbyDto> CreateAsync(HobbyCreateDto input);

        Task<HobbyDto> UpdateAsync(Guid id, HobbyUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(HobbyExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}