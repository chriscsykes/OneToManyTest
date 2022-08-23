using OneToManyTest.Shared;
using OneToManyTest.Orders;
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
using OneToManyTest.Customers;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using OneToManyTest.Shared;

namespace OneToManyTest.Customers
{

    [Authorize(OneToManyTestPermissions.Customers.Default)]
    public class CustomersAppService : ApplicationService, ICustomersAppService
    {
        private readonly IDistributedCache<CustomerExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly ICustomerRepository _customerRepository;
        private readonly CustomerManager _customerManager;
        private readonly IRepository<Order, Guid> _orderRepository;

        public CustomersAppService(ICustomerRepository customerRepository, CustomerManager customerManager, IDistributedCache<CustomerExcelDownloadTokenCacheItem, string> excelDownloadTokenCache, IRepository<Order, Guid> orderRepository)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _customerRepository = customerRepository;
            _customerManager = customerManager; _orderRepository = orderRepository;
        }

        public virtual async Task<PagedResultDto<CustomerWithNavigationPropertiesDto>> GetListAsync(GetCustomersInput input)
        {
            var totalCount = await _customerRepository.GetCountAsync(input.FilterText, input.FirstName, input.LastName, input.Email, input.Address, input.OrderId);
            var items = await _customerRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.FirstName, input.LastName, input.Email, input.Address, input.OrderId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<CustomerWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<CustomerWithNavigationProperties>, List<CustomerWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<CustomerWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<CustomerWithNavigationProperties, CustomerWithNavigationPropertiesDto>
                (await _customerRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<CustomerDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Customer, CustomerDto>(await _customerRepository.GetAsync(id));
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid?>>> GetOrderLookupAsync(LookupRequestDto input)
        {
            var query = (await _orderRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.Item != null &&
                         x.Item.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Order>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid?>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Order>, List<LookupDto<Guid?>>>(lookupData)
            };
        }

        [Authorize(OneToManyTestPermissions.Customers.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _customerRepository.DeleteAsync(id);
        }

        [Authorize(OneToManyTestPermissions.Customers.Create)]
        public virtual async Task<CustomerDto> CreateAsync(CustomerCreateDto input)
        {

            var customer = await _customerManager.CreateAsync(
            input.OrderId, input.FirstName, input.LastName, input.Email, input.Address
            );

            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        [Authorize(OneToManyTestPermissions.Customers.Edit)]
        public virtual async Task<CustomerDto> UpdateAsync(Guid id, CustomerUpdateDto input)
        {

            var customer = await _customerManager.UpdateAsync(
            id,
            input.OrderId, input.FirstName, input.LastName, input.Email, input.Address, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(CustomerExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _customerRepository.GetListAsync(input.FilterText, input.FirstName, input.LastName, input.Email, input.Address);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Customer>, List<CustomerExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Customers.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new CustomerExcelDownloadTokenCacheItem { Token = token },
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