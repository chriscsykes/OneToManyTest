using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace OneToManyTest.Customers
{
    public class CustomerDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Guid? OrderId { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}