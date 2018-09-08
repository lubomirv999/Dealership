namespace Dealership.Services
{
    using Dealership.Data;
    using Dealership.Data.Enums;
    using System.Collections.Generic;

    public interface ICarService
    {
        IEnumerable<Car> All();

        bool Add(string manufacturer, string model, short yearOfProduction, BodyType bodyType, Condition condition, TypeOfTransmission typeOfTransmission, EuroStandart euroStandart, EngineType engineType, int travelledDistance, short horsePower, string color, string saleDescription, decimal price, string imagesUrls);

        bool Edit(int id, string manufacturer, string model, short yearOfProduction, BodyType bodyType, Condition condition, TypeOfTransmission typeOfTransmission, EuroStandart euroStandart, EngineType engineType, int travelledDistance, short horsePower, string color, string saleDescription, decimal price, string imagesUrls);

        bool Delete(int IdToRemove);              

        bool Exists(int id);

        Car FindById(int id);
    }
}
