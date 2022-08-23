using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using OneToManyTest.Shared;

namespace OneToManyTest.Orders
{
    public interface IOrdersAppService : IApplicationService
    {
        Task<PagedResultDto<OrderDto>> GetListAsync(GetOrdersInput input);

        Task<OrderDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<OrderDto> CreateAsync(OrderCreateDto input);

        Task<OrderDto> UpdateAsync(Guid id, OrderUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(OrderExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}