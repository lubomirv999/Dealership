namespace Dealership.Services
{
    using Dealership.Data;
    using Dealership.Models.CarModels;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Net.Mail;

    public class EmailService : IEmailService
    {
        private IConfiguration Configuration { get; }

        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void SendEMail(BuyCarFormModel personInfo, Car carToBuy)
        {
            string id = Configuration.GetSection("EmailId").Value.ToString();
            string password = Configuration.GetSection("EmailPassword").Value.ToString();
            string msg = $"Person with name {personInfo.FirstName} {personInfo.LastName}{Environment.NewLine}Wants to buy car #{carToBuy.Id} ({carToBuy.Manufacturer} {carToBuy.Model}){Environment.NewLine}Contact him on Phone: {personInfo.GSM} or E-Mail: {personInfo.Email}{Environment.NewLine}Comment: {personInfo.Comment}";
            string emailTitle = $@"{carToBuy.Manufacturer} {carToBuy.Model} ID# {carToBuy.Id}";

            SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential(id, password),
                Timeout = 10000,
            };

            MailMessage mm = new MailMessage(id, id, emailTitle, msg);
            client.Send(mm);
        }
    }
}
