using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace OneToManyTest.Hobbies
{
    public class HobbyUpdateDto : IHasConcurrencyStamp
    {
        [Required]
        public string Name { get; set; }
        public int YearsPerformed { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}