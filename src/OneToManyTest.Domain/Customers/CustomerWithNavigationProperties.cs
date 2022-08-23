using OneToManyTest.Orders;

using System;
using System.Collections.Generic;

namespace OneToManyTest.Customers
{
    public class CustomerWithNavigationProperties
    {
        public Customer Customer { get; set; }

        public Order Order { get; set; }
        

        
    }
}