using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace OneToManyTest.Orders
{
    public class OrderDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Item { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}