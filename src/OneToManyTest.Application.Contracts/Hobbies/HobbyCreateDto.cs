using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace OneToManyTest.Hobbies
{
    public class HobbyCreateDto
    {
        [Required]
        public string Name { get; set; }
        public int YearsPerformed { get; set; } = 1;
        public List<Guid> CustomerIds { get; set; }
    }
}