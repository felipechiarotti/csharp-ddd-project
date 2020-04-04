 using Domain.ValueObjects;
using Flunt.Validations;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class Student : Entity
    {
        public Name Name { get; set; }
        public Document Document { get; set; }
        public Email Email { get; private set; }
        public Address Address { get; private set; }
        public IReadOnlyCollection<Subscription> Subscriptions { get { return _subscriptions.ToArray(); } }
        private IList<Subscription> _subscriptions;

        public Student(Name name, Document document, Email email)
        {
            Name = name;
            Document = document;
            Email = email;
            _subscriptions = new List<Subscription>();

            AddNotifications(name, document, email);
        }

        public void AddSubscription(Subscription subscription)
        {
            var hasSubscriptionActive = false;
            foreach(var sub in _subscriptions)
            {
                if (sub.Active)
                {
                    hasSubscriptionActive = true;
                    break;
                }
            }

            //if (hasSubscriptionActive)
            //    AddNotification("Student.Subscriptions", "Você já tem uma assinatura ativa");
            AddNotifications(new Contract()
                .Requires()
                .IsTrue(hasSubscriptionActive, "Student.Subscriptions", "Você já tem uma assinatura ativa")
                .IsLowerOrEqualsThan(0, subscription.Payments.Count, "Student.Subscription.Payments", "Esta assinatura não possui pagamentos")
                );
        }
    }
}
