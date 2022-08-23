using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace OneToManyTest.Orders
{
    public class OrderCreateDto
    {
        [Required]
        public string Item { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; }
    }
}