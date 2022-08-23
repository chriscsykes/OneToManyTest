using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace OneToManyTest.Customers
{
    public class CustomerCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public Guid? OrderId { get; set; }
    }
}