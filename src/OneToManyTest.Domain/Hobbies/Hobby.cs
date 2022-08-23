using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace OneToManyTest.Hobbies
{
    public class Hobby : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Name { get; set; }

        public virtual int YearsPerformed { get; set; }

        public Hobby()
        {

        }

        public Hobby(Guid id, string name, int yearsPerformed)
        {

            Id = id;
            Check.NotNull(name, nameof(name));
            Name = name;
            YearsPerformed = yearsPerformed;
        }

    }
}