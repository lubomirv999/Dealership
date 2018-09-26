namespace Dealership.Services
{
    using Dealership.Data;
    using Dealership.Models.CarModels;
    using Microsoft.Extensions.Configuration;
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

            SmtpClient client = new SmtpClient
            {
                Host = Configuration.GetSection("Host").Value.ToString(),
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential(id, password),
                Timeout = 10000,
            };

            MailMessage mm = new MailMessage(id, id, carToBuy.Manufacturer + " " + carToBuy.Model + " with id #" + carToBuy.Id,
                "Person with name " + personInfo.FirstName + " " + personInfo.LastName
                + "\nWants to buy car #" + carToBuy.Id + "(" + carToBuy.Manufacturer + " " + carToBuy.Model + "). "
                + "\nContact him on phone : " + personInfo.GSM + " or E-Mail : " + personInfo.Email
                + "\nComment: " + personInfo.Comment);

            client.Send(mm);
        }
    }
}
