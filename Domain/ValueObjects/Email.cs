using Shared.ValueObjects;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Text;
using Flunt.Notifications;

namespace Domain.ValueObjects
{
    public class Email : ValueObject
    {

        public Email(string address)
        {
            Address = address;

            AddNotifications(new Contract()
                .Requires()
                .IsEmail(Address, "Email.Address", "E-mail inválido")
            );
        }

        public string Address { get; private set; }
    }
}
