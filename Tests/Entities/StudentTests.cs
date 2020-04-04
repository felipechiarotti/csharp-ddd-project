using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Entities
{
    [TestClass]
    public class StudentTests
    {
        private readonly Name _name;
        private readonly Document _document;
        private readonly Email _email;
        private readonly Address _address;
        private readonly Student _student;
        private readonly Subscription _subscription;

        public StudentTests()
        {
            var _name = new Name("Felipe", "Chiarotti");
            var _document = new Document("10575342935", Domain.Enums.EDocumentType.CPF);
            var _email = new Email("felipechi97@gmail.com");
            var _address = new Address("Rua 1", "900", "Centro", "Ribeirão", "PR", "Brasil", "86410000");
            _student = new Student(_name, _document, _email);

            _subscription = new Subscription(null);


        }
        [TestMethod]
        public void ShouldReturnErrorWhenHadActiveSubscription()
        {
            var payment = new PaypalPayment("12345678", DateTime.Now, DateTime.Now.AddDays(30), 10, 10, "WAYNE CORP", _document, _address, _email);
            _subscription.AddPayment(payment);
            _student.AddSubscription(_subscription);
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Invalid);
        }

        [TestMethod]
        public void ShouldReturnErrorWhenSubscriptionHasNoPayment()
        {
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Invalid);
        }
    }


}
