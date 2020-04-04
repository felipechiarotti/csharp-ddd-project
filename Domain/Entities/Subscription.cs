using Flunt.Validations;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class Subscription : Entity
    {
        public DateTime CreateDate { get; private set; }
        public DateTime LastUpdateDate {get; private set; }
        public DateTime? ExpireDate {get; private set; }
        public bool Active { get; private set; }
        public IReadOnlyCollection<Payment> Payments { get { return _payments.ToArray(); } }
        private IList<Payment> _payments;


        public Subscription(DateTime? expireDate)
        {
            CreateDate = DateTime.Now;
            LastUpdateDate = DateTime.Now;
            ExpireDate = expireDate;
            Active = false;
            _payments = new List<Payment>(); 
        }

        public void AddPayment(Payment payment)
        {
            _payments.Add(payment);
        }

        public void Activate(bool active)
        {
            Active = active;
            LastUpdateDate = DateTime.Now;
        }

    }
}
