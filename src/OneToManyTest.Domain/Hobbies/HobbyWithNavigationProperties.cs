using OneToManyTest.Customers;

using System;
using System.Collections.Generic;

namespace OneToManyTest.Hobbies
{
    public class HobbyWithNavigationProperties
    {
        public Hobby Hobby { get; set; }

        

        public List<Customer> Customers { get; set; }
        
    }
}