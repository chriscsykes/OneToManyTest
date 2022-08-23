using OneToManyTest.Shared;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using OneToManyTest.Shared;

namespace OneToManyTest.Customers
{
    public interface ICustomersAppService : IApplicationService
    {
        Task<PagedResultDto<CustomerWithNavigationPropertiesDto>> GetListAsync(GetCustomersInput input);

        Task<CustomerWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<CustomerDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid?>>> GetOrderLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<CustomerDto> CreateAsync(CustomerCreateDto input);

        Task<CustomerDto> UpdateAsync(Guid id, CustomerUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(CustomerExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}