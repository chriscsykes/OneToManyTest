using OneToManyTest.Orders;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace OneToManyTest.Customers
{
    public class CustomerWithNavigationPropertiesDto
    {
        public CustomerDto Customer { get; set; }

        public OrderDto Order { get; set; }

    }
}