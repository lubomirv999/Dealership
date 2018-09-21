namespace Dealership.Services
{
    using Dealership.Data;
    using Dealership.Models.CarModels;

    public interface IEmailService
    {
        void SendEMail(BuyCarFormModel personInfo, Car carToBuy);
    }
}
