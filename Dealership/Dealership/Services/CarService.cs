namespace Dealership.Services
{
    using Dealership.Data;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class CarService : ICarService
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly DealershipDbContext db;

        public CarService(IHostingEnvironment hostingEnvironment, DealershipDbContext db)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.db = db;
        }

        public void Add(Car carEntity, ICollection<IFormFile> images)
        {
            List<Image> dataImages = new List<Image>();

            foreach (var image in images)
            {
                if (image != null)
                {
                    var imgGuid = Guid.NewGuid();
                    string img = Path.GetFileName(image.FileName);
                    string imgMimeType = image.ContentType.Split("/").Last();
                    string path = Path.Combine(hostingEnvironment.WebRootPath + "\\images", imgGuid.ToString() + "." + imgMimeType);

                    using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        image.CopyTo(fs);
                    }

                    var dataImg = new Image();
                    dataImg.ImageUrl = imgGuid + "." + imgMimeType;
                    dataImages.Add(dataImg);
                }
            }

            //this.db.Images.AddRange(dataImages);
            carEntity.Images = dataImages;

            this.db.Cars.Add(carEntity);
            this.db.SaveChanges();
        }

        public void Edit(int id, Car carEntity, ICollection<IFormFile> images)
        {
            var car = this.db.Cars.Find(id);

            List<Image> dataImages = new List<Image>();

            foreach (var image in images)
            {
                if (image != null)
                {
                    var imgGuid = Guid.NewGuid();
                    string img = Path.GetFileName(image.FileName);
                    string imgMimeType = image.ContentType.Split("/").Last();
                    string path = Path.Combine(hostingEnvironment.WebRootPath + "\\images", imgGuid.ToString() + "." + imgMimeType);

                    using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        image.CopyTo(fs);
                    }

                    var dataImg = new Image();
                    dataImg.ImageUrl = imgGuid + "." + imgMimeType;
                    dataImages.Add(dataImg);
                }
            }

            //this.db.Images.AddRange(dataImages);

            car.Manufacturer = carEntity.Manufacturer;
            car.Model = carEntity.Model;
            car.YearOfProduction = carEntity.YearOfProduction;
            car.BodyType = carEntity.BodyType;
            car.Condition = carEntity.Condition;
            car.TypeOfTransmission = carEntity.TypeOfTransmission;
            car.EuroStandart = carEntity.EuroStandart;
            car.EngineType = carEntity.EngineType;
            car.TravelledDistance = carEntity.TravelledDistance;
            car.HorsePower = carEntity.HorsePower;
            car.Color = carEntity.Color;
            car.SaleDescription = carEntity.SaleDescription;
            car.Price = carEntity.Price;
            car.Images = dataImages;

            this.db.SaveChanges();
        }

        public void Delete(int carId)
        {
            Car carToDelete = this.db.Cars.Include("Images").FirstOrDefault(p => p.Id == carId);

            foreach (var imageToDelete in carToDelete.Images)
            {
                string pathToDelete = hostingEnvironment.WebRootPath + "\\images\\" + imageToDelete.ImageUrl;

                System.IO.File.Delete(pathToDelete);
            }

            db.Cars.Remove(carToDelete);
            db.SaveChanges();
        }

        public IEnumerable<Car> All(string sort)
        {
            switch (sort)
            {
                case "manufacturer":
                    return this.db.Cars.OrderBy(c => c.Manufacturer).Include("Images");
                case "condition":
                    return this.db.Cars.OrderBy(c => c.Condition).Include("Images");
                case "year":
                    return this.db.Cars.OrderBy(c => c.YearOfProduction).Include("Images");
                case "priceAsc":
                    return this.db.Cars.OrderBy(c => c.Price).Include("Images");
                case "priceDesc":
                    return this.db.Cars.OrderByDescending(c => c.Price).Include("Images");
                default:
                    return this.db.Cars.Include("Images");
            }
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
            if (string.IsNullOrEmpty(searchQuery))
            {
                return All(null);
            }

            return this.db.Cars.Where(c => c.Manufacturer.Contains(searchQuery) || c.Model.Contains(searchQuery) || c.SaleDescription.Contains(searchQuery)).Include("Images");
        }

        public void DeletePhoto(int photoId)
        {
            Image imageToDelete = this.db.Images.FirstOrDefault(p => p.Id == photoId);

            string pathToDelete = hostingEnvironment.WebRootPath + "\\images\\" + imageToDelete.ImageUrl;

            System.IO.File.Delete(pathToDelete);

            db.Images.Remove(imageToDelete);
            db.SaveChanges();
        }
    }
}
