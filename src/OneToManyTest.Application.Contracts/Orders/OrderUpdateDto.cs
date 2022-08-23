using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace OneToManyTest.Orders
{
    public class OrderUpdateDto : IHasConcurrencyStamp
    {
        [Required]
        public string Item { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}