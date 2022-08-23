using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace OneToManyTest.Hobbies
{
    public class HobbyDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Name { get; set; }
        public int YearsPerformed { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}