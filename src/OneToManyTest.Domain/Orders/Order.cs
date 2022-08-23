using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace OneToManyTest.Orders
{
    public class Order : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Item { get; set; }

        public virtual int Quantity { get; set; }

        public virtual decimal Price { get; set; }

        public Order()
        {

        }

        public Order(Guid id, string item, int quantity, decimal price)
        {

            Id = id;
            Check.NotNull(item, nameof(item));
            Item = item;
            Quantity = quantity;
            Price = price;
        }

    }
}