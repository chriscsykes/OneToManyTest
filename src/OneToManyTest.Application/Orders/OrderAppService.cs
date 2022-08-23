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
using OneToManyTest.Orders;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using OneToManyTest.Shared;

namespace OneToManyTest.Orders
{

    [Authorize(OneToManyTestPermissions.Orders.Default)]
    public class OrdersAppService : ApplicationService, IOrdersAppService
    {
        private readonly IDistributedCache<OrderExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IOrderRepository _orderRepository;
        private readonly OrderManager _orderManager;

        public OrdersAppService(IOrderRepository orderRepository, OrderManager orderManager, IDistributedCache<OrderExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _orderRepository = orderRepository;
            _orderManager = orderManager;
        }

        public virtual async Task<PagedResultDto<OrderDto>> GetListAsync(GetOrdersInput input)
        {
            var totalCount = await _orderRepository.GetCountAsync(input.FilterText, input.Item, input.QuantityMin, input.QuantityMax, input.PriceMin, input.PriceMax);
            var items = await _orderRepository.GetListAsync(input.FilterText, input.Item, input.QuantityMin, input.QuantityMax, input.PriceMin, input.PriceMax, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<OrderDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Order>, List<OrderDto>>(items)
            };
        }

        public virtual async Task<OrderDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Order, OrderDto>(await _orderRepository.GetAsync(id));
        }

        [Authorize(OneToManyTestPermissions.Orders.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        [Authorize(OneToManyTestPermissions.Orders.Create)]
        public virtual async Task<OrderDto> CreateAsync(OrderCreateDto input)
        {

            var order = await _orderManager.CreateAsync(
            input.Item, input.Quantity, input.Price
            );

            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        [Authorize(OneToManyTestPermissions.Orders.Edit)]
        public virtual async Task<OrderDto> UpdateAsync(Guid id, OrderUpdateDto input)
        {

            var order = await _orderManager.UpdateAsync(
            id,
            input.Item, input.Quantity, input.Price, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(OrderExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _orderRepository.GetListAsync(input.FilterText, input.Item, input.QuantityMin, input.QuantityMax, input.PriceMin, input.PriceMax);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Order>, List<OrderExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Orders.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new OrderExcelDownloadTokenCacheItem { Token = token },
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