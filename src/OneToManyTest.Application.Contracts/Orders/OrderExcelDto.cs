using System;

namespace OneToManyTest.Orders
{
    public class OrderExcelDto
    {
        public string Item { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}