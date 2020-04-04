using Domain.Commands;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;
using Flunt.Notifications;
using Shared.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Handlers
{
    public class SubscriptionHandler : Notifiable,
        IHandler<CreateBoletoSubscriptionCommand>,
        IHandler<CreatePayPalSubscriptionCommand>
    {

        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {

            //Fail fast validations
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar assinatura");
            }

            // Verificar se Documento já está cadastrado
            if (_repository.DocumentExists(command.PayerDocument))
                AddNotification("Document", "Este CPF já está em uso");

            //Verificar se E-mail já está cadastrado
            if (_repository.EmailExists(command.Email))
                AddNotification("Email", "Este email já está sendo utilizado");

            // Gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.PayerDocument, Domain.Enums.EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.HouseNumber, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            // Gerar as entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddDays(30));
            var payment = new BoletoPayment(
                command.BarCode,
                command.BoletoNumber,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument, command.PayerDocumentType),
                address,
                email
            );


            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            // Agrupar as validações
            AddNotifications(name, document, email, address, student, subscription, payment);

            // Checar as notificações
            if (Invalid)
                return new CommandResult(false, "Não foi possível realizar a assinatura");
            
            // Salvar as informações
            _repository.CreateSubscription(student);

            // Enviar Email de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ao módulo", "Sua assinatura foi criada");
            
            // Retornar informações
            return new CommandResult(true, "Assinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {

            // Verificar se Documento já está cadastrado
            if (_repository.DocumentExists(command.PayerDocument))
                AddNotification("Document", "Este CPF já está em uso");

            //Verificar se E-mail já está cadastrado
            if (_repository.EmailExists(command.Email))
                AddNotification("Email", "Este email já está sendo utilizado");

            // Gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.PayerDocument, Domain.Enums.EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.HouseNumber, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            // Gerar as entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddDays(30));
            var payment = new PaypalPayment(
                command.TransactionCode,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument, command.PayerDocumentType),
                address,
                email
            );


            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            // Agrupar as validações
            AddNotifications(name, document, email, address, student, subscription, payment);

            // Checar as notificações
            if (Invalid)
                return new CommandResult(false, "Não foi possível realizar a assinatura");

            // Salvar as informações
            _repository.CreateSubscription(student);

            // Enviar Email de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ao módulo", "Sua assinatura foi criada");

            // Retornar informações
            return new CommandResult(true, "Assinatura realizada com sucesso");
        }
    }
}
