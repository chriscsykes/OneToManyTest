using OneToManyTest.Orders;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace OneToManyTest.Customers
{
    public class Customer : FullAuditedAggregateRoot<Guid>
    {
        [CanBeNull]
        public virtual string FirstName { get; set; }

        [CanBeNull]
        public virtual string LastName { get; set; }

        [NotNull]
        public virtual string Email { get; set; }

        [CanBeNull]
        public virtual string Address { get; set; }
        public Guid? OrderId { get; set; }

        public Customer()
        {

        }

        public Customer(Guid id, Guid? orderId, string firstName, string lastName, string email, string address)
        {

            Id = id;
            Check.NotNull(email, nameof(email));
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Address = address;
            OrderId = orderId;
        }

    }
}