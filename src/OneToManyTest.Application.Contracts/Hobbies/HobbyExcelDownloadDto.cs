using Volo.Abp.Application.Dtos;
using System;

namespace OneToManyTest.Hobbies
{
    public class HobbyExcelDownloadDto
    {
        public string DownloadToken { get; set; }

        public string FilterText { get; set; }

        public string Name { get; set; }
        public int? YearsPerformedMin { get; set; }
        public int? YearsPerformedMax { get; set; }

        public HobbyExcelDownloadDto()
        {

        }
    }
}