using OneToManyTest.Hobbies;
using OneToManyTest.Orders;
using System;
using OneToManyTest.Shared;
using Volo.Abp.AutoMapper;
using OneToManyTest.Customers;
using AutoMapper;

namespace OneToManyTest;

public class OneToManyTestApplicationAutoMapperProfile : Profile
{
    public OneToManyTestApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Customer, CustomerDto>();
        CreateMap<Customer, CustomerExcelDto>();

        CreateMap<Order, OrderDto>();
        CreateMap<Order, OrderExcelDto>();

        CreateMap<CustomerWithNavigationProperties, CustomerWithNavigationPropertiesDto>();
        CreateMap<Order, LookupDto<Guid?>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Item));

        CreateMap<Hobby, HobbyDto>();
        CreateMap<Hobby, HobbyExcelDto>();
    }
}