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

        public ICollection<HobbyCustomer> Customers { get; private set; }

        public Hobby()
        {

        }

        public Hobby(Guid id, string name, int yearsPerformed)
        {

            Id = id;
            Check.NotNull(name, nameof(name));
            Name = name;
            YearsPerformed = yearsPerformed;
            Customers = new Collection<HobbyCustomer>();
        }
        public void AddCustomer(Guid customerId)
        {
            Check.NotNull(customerId, nameof(customerId));

            if (IsInCustomers(customerId))
            {
                return;
            }

            Customers.Add(new HobbyCustomer(Id, customerId));
        }

        public void RemoveCustomer(Guid customerId)
        {
            Check.NotNull(customerId, nameof(customerId));

            if (!IsInCustomers(customerId))
            {
                return;
            }

            Customers.RemoveAll(x => x.CustomerId == customerId);
        }

        public void RemoveAllCustomersExceptGivenIds(List<Guid> customerIds)
        {
            Check.NotNullOrEmpty(customerIds, nameof(customerIds));

            Customers.RemoveAll(x => !customerIds.Contains(x.CustomerId));
        }

        public void RemoveAllCustomers()
        {
            Customers.RemoveAll(x => x.HobbyId == Id);
        }

        private bool IsInCustomers(Guid customerId)
        {
            return Customers.Any(x => x.CustomerId == customerId);
        }
    }
}