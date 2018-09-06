namespace Dealership.Services
{
    using Dealership.Data.Enums;

    public interface ICarService
    {
        bool Add(string manufacturer, string model, short yearOfProduction, BodyType bodyType, Condition condition, TypeOfTransmission typeOfTransmission, EuroStandart euroStandart, EngineType engineType, int travelledDistance, short horsePower, string color, string saleDescription, decimal price, string imagesUrls);
    }
}
