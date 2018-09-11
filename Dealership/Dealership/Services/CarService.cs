namespace Dealership.Services
{
    using Dealership.Data;
    using Dealership.Models.CarViewModels;
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

        public void Add(Car addCarModel, ICollection<IFormFile> images)
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

            this.db.Images.AddRange(dataImages);
            addCarModel.Images = dataImages;

            this.db.Cars.Add(addCarModel);
            this.db.SaveChanges();
        }

        public void Edit(int id, Car editCarModel, ICollection<IFormFile> images)
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

            this.db.Images.AddRange(dataImages);

            car.Manufacturer = editCarModel.Manufacturer;
            car.Model = editCarModel.Model;
            car.YearOfProduction = editCarModel.YearOfProduction;
            car.BodyType = editCarModel.BodyType;
            car.Condition = editCarModel.Condition;
            car.TypeOfTransmission = editCarModel.TypeOfTransmission;
            car.EuroStandart = editCarModel.EuroStandart;
            car.EngineType = editCarModel.EngineType;
            car.TravelledDistance = editCarModel.TravelledDistance;
            car.HorsePower = editCarModel.HorsePower;
            car.Color = editCarModel.Color;
            car.SaleDescription = editCarModel.SaleDescription;
            car.Price = editCarModel.Price;
            car.Images = dataImages;

            this.db.SaveChanges();
        }

        public void Delete(int carId)
        {
            Car carToDelete = this.db.Cars.FirstOrDefault(p => p.Id == carId);

            db.Cars.Remove(carToDelete);
            db.SaveChanges();
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
            if (string.IsNullOrEmpty(searchQuery))
            {
                return All();
            }

            return this.db.Cars.Where(c => c.Manufacturer.Contains(searchQuery) || c.Model.Contains(searchQuery) || c.SaleDescription.Contains(searchQuery)).Include("Images");
        }
    }
}
