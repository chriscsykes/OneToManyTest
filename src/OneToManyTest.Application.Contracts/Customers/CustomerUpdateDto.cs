using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace OneToManyTest.Customers
{
    public class CustomerUpdateDto : IHasConcurrencyStamp
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public Guid? OrderId { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}