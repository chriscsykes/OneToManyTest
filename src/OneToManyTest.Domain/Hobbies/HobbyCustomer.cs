using System;
using Volo.Abp.Domain.Entities;

namespace OneToManyTest.Hobbies
{
    public class HobbyCustomer : Entity
    {

        public Guid HobbyId { get; protected set; }

        public Guid CustomerId { get; protected set; }

        private HobbyCustomer()
        {

        }

        public HobbyCustomer(Guid hobbyId, Guid customerId)
        {
            HobbyId = hobbyId;
            CustomerId = customerId;
        }

        public override object[] GetKeys()
        {
            return new object[]
                {
                    HobbyId,
                    CustomerId
                };
        }
    }
}