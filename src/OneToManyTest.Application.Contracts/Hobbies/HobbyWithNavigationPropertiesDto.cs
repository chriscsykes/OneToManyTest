using OneToManyTest.Customers;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace OneToManyTest.Hobbies
{
    public class HobbyWithNavigationPropertiesDto
    {
        public HobbyDto Hobby { get; set; }

        public List<CustomerDto> Customers { get; set; }

    }
}