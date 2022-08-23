using OneToManyTest.Hobbies;
using OneToManyTest.Orders;
using Volo.Abp.AutoMapper;
using OneToManyTest.Customers;
using AutoMapper;

namespace OneToManyTest.Blazor;

public class OneToManyTestBlazorAutoMapperProfile : Profile
{
    public OneToManyTestBlazorAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Blazor project.

        CreateMap<CustomerDto, CustomerUpdateDto>();

        CreateMap<OrderDto, OrderUpdateDto>();

        CreateMap<HobbyDto, HobbyUpdateDto>();

        CreateMap<HobbyDto, HobbyUpdateDto>().Ignore(x => x.CustomerIds);
    }
}