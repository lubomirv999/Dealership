namespace Dealership.Services
{
    using Dealership.Data;
    using Dealership.Data.Enums;
    using System.Collections.Generic;

    public class CarService : ICarService
    {
        private readonly DealershipDbContext db;

        public CarService(DealershipDbContext db)
        {
            this.db = db;
        }

        public bool Add(string manufacturer, string model, short yearOfProduction, BodyType bodyType, Condition condition, TypeOfTransmission typeOfTransmission, EuroStandart euroStandart, EngineType engineType, int travelledDistance, short horsePower, string color, string saleDescription, decimal price, string imagesUrls)
        {
            List<Image> images = new List<Image>();

            var imagesCount = imagesUrls.Split(", ");

            for (int i = 0; i < imagesCount.Length; i++)
            {
                Image image = new Image();
                image.ImageUrl = imagesCount[i];
                images.Add(image);
                this.db.Images.Add(image);
            }

            var car = new Car
            {
                Manufacturer = manufacturer,
                Model = model,
                YearOfProduction = yearOfProduction,
                BodyType = bodyType,
                Condition = condition,
                TypeOfTransmission = typeOfTransmission,
                EuroStandart = euroStandart,
                EngineType = engineType,
                TravelledDistance = travelledDistance,
                HorsePower = horsePower,
                Color = color,
                SaleDescription = saleDescription,
                Price = price,
                Images = images
            };

            this.db.Cars.Add(car);
            this.db.SaveChanges();

            return true;
        }
    }
}
