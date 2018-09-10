namespace Dealership.Services
{
    using Dealership.Data;
    using Dealership.Data.Enums;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;

    public class CarService : ICarService
    {
        private readonly DealershipDbContext db;

        public CarService(DealershipDbContext db)
        {
            this.db = db;
        }

        public bool Add(string manufacturer, string model, short yearOfProduction, BodyType bodyType, Condition condition, TypeOfTransmission typeOfTransmission, EuroStandart euroStandart, EngineType engineType, int travelledDistance, short horsePower, string color, string saleDescription, decimal price, string imagesUrls)
        {
            try
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
            catch
            {
                return false;
            }
        }

        public bool Edit(int id, string manufacturer, string model, short yearOfProduction, BodyType bodyType, Condition condition, TypeOfTransmission typeOfTransmission, EuroStandart euroStandart, EngineType engineType, int travelledDistance, short horsePower, string color, string saleDescription, decimal price, string imagesUrls)
        {
            try
            {
                if (!this.db.Cars.Any(p => p.Id == id))
                {
                    return false;
                }

                var car = this.db.Cars.Find(id);

                if (car == null)
                {
                    return false;
                }

                List<Image> images = new List<Image>();

                var imagesCount = imagesUrls.Split(", ");

                for (int i = 0; i < imagesCount.Length; i++)
                {
                    Image image = new Image();
                    image.ImageUrl = imagesCount[i];
                    images.Add(image);

                    foreach (var img in this.db.Images)
                    {
                        if (!(img.Id == image.Id))
                        {
                            this.db.Images.Add(image);
                        }
                    }
                }

                car.Manufacturer = manufacturer;
                car.Model = model;
                car.YearOfProduction = yearOfProduction;
                car.BodyType = bodyType;
                car.Condition = condition;
                car.TypeOfTransmission = typeOfTransmission;
                car.EuroStandart = euroStandart;
                car.EngineType = engineType;
                car.TravelledDistance = travelledDistance;
                car.HorsePower = horsePower;
                car.Color = color;
                car.SaleDescription = saleDescription;
                car.Price = price;
                car.Images = images;

                this.db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int carId)
        {
            try
            {
                Car carToDelete = db.Cars.FirstOrDefault(p => p.Id == carId);

                if (carToDelete != null)
                {
                    db.Cars.Remove(carToDelete);
                    db.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Car> All()
        {
            return this.db.Cars.Include("Images");
        }

        public Car FindById(int id)
            => this.db
                .Cars
                .Include("Images")
                .Where(c => c.Id == id)
                .FirstOrDefault();

        public bool Exists(int id)
            => this.db.Cars.Any(c => c.Id == id);

        public IEnumerable<Car> Search(string searchQuery)
        {
            if(string.IsNullOrEmpty(searchQuery))
            {
                return All();
            }

            return this.db.Cars.Where(c => c.Manufacturer.Contains(searchQuery) || c.Model.Contains(searchQuery) || c.SaleDescription.Contains(searchQuery)).Include("Images");
        }
    }
}
