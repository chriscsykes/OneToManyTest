using Volo.Abp.Application.Dtos;
using System;

namespace OneToManyTest.Hobbies
{
    public class GetHobbiesInput : PagedAndSortedResultRequestDto
    {
        public string FilterText { get; set; }

        public string Name { get; set; }
        public int? YearsPerformedMin { get; set; }
        public int? YearsPerformedMax { get; set; }

        public GetHobbiesInput()
        {

        }
    }
}