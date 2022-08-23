using Volo.Abp.Application.Dtos;
using System;

namespace OneToManyTest.Customers
{
    public class GetCustomersInput : PagedAndSortedResultRequestDto
    {
        public string FilterText { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Guid? OrderId { get; set; }

        public GetCustomersInput()
        {

        }
    }
}