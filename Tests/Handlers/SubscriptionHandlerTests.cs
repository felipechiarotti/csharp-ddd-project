using Domain.Commands;
using Domain.Enums;
using Domain.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Tests.Mooks;

namespace Tests.Handlers
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "Bruce";
            command.LastName = "Wayne";
            command.PayerDocument = "99999999999";
            command.Email = "felipechi97@gmail.com";

            command.BarCode = "123456789";
            command.BoletoNumber = "12345678987";
            command.PaymentNumber = "12321";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 60;
            command.TotalPaid = 60;
            command.Payer = "Wayne Corp";
            command.PayerDocumentType = EDocumentType.CPF;
            command.Street = "asdas";
            command.HouseNumber = "asdsd";
            command.Neighborhood = "as";
            command.City = "as";
            command.State = "as";
            command.Country = "as";
            command.ZipCode = "as";

            handler.Handle(command);
            Assert.AreEqual(false, handler.Valid);

        }
    }
}
