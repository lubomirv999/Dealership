namespace Dealership.Services
{
    using Dealership.Data;
    using Dealership.Models.CarModels;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class CarService : ICarService
    {
        private readonly DealershipDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;        

        public CarService(DealershipDbContext db, IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
            this._db = db;
        }

        public void Add(Car carEntity, ICollection<IFormFile> images)
        {
            List<Image> dataImages = new List<Image>();

            foreach (var image in images)
            {
                if (image != null)
                {
                    string imgMimeType = image.ContentType.Split("/").Last();

                    if (imgMimeType == "jpg" || imgMimeType == "png" || imgMimeType == "jpeg")
                    {
                        var imgGuid = Guid.NewGuid();
                        string img = Path.GetFileName(image.FileName);
                        string path = Path.Combine(_hostingEnvironment.WebRootPath + "\\images", imgGuid.ToString() + "." + imgMimeType);

                        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                        {
                            image.CopyTo(fs);
                        }

                        var dataImg = new Image();
                        dataImg.ImageUrl = imgGuid + "." + imgMimeType;
                        dataImages.Add(dataImg);
                    }
                }
            }

            carEntity.Images = dataImages;

            this._db.Cars.Add(carEntity);
            this._db.SaveChanges();
        }

        public void Edit(int id, Car carEntity, ICollection<IFormFile> images)
        {
            var car = this._db.Cars.Find(id);

            List<Image> dataImages = new List<Image>();

            foreach (var image in images)
            {
                if (image != null)
                {
                    string imgMimeType = image.ContentType.Split("/").Last();

                    if (imgMimeType == "jpg" || imgMimeType == "png" || imgMimeType == "jpeg")
                    {

                        var imgGuid = Guid.NewGuid();
                        string img = Path.GetFileName(image.FileName);
                        string path = Path.Combine(_hostingEnvironment.WebRootPath + "\\images", imgGuid.ToString() + "." + imgMimeType);

                        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                        {
                            image.CopyTo(fs);
                        }

                        var dataImg = new Image();
                        dataImg.ImageUrl = imgGuid + "." + imgMimeType;
                        dataImages.Add(dataImg);
                    }
                }
            }

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

            this._db.SaveChanges();
        }

        public void Delete(int carId)
        {
            Car carToDelete = this._db.Cars.Include("Images").FirstOrDefault(p => p.Id == carId);

            foreach (var imageToDelete in carToDelete.Images)
            {
                string pathToDelete = _hostingEnvironment.WebRootPath + "\\images\\" + imageToDelete.ImageUrl;

                File.Delete(pathToDelete);
            }

            _db.Cars.Remove(carToDelete);
            _db.SaveChanges();
        }

        public AllCarsListModel All(string sort, string searchQuery, int pageSize, int page = 1)
        {
            var model = new AllCarsListModel();
            model = SearchCars(searchQuery, pageSize, page);

            switch (sort)
            {
                case "Manufacturer":
                    model.Cars = model
                        .Cars
                        .OrderBy(c => c.Manufacturer);
                    break;
                case "Condition":
                    model.Cars = model
                        .Cars
                        .OrderBy(c => c.Condition);
                    break;
                case "Year":
                    model.Cars = model
                        .Cars
                        .OrderBy(c => c.YearOfProduction);
                    break;
                case "Price Asc":
                    model.Cars = model
                        .Cars
                        .OrderBy(c => c.Price);
                    break;
                case "Price Desc":
                    model.Cars = model
                        .Cars
                        .OrderByDescending(c => c.Price);
                    break;
                default:
                    break;
            }

            return model;
        }

        public AllCarsListModel SearchCars(string searchQuery, int pageSize, int page)
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                var model = new AllCarsListModel();

                model.Cars = this._db.Cars.Where(c =>
                   c.Manufacturer.Contains(searchQuery)
                   || c.Model.Contains(searchQuery)
                   || c.SaleDescription.Contains(searchQuery))
                   .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Include("Images")
                    .ToList();
                model.Count = this._db.Cars.Where(c =>
                   c.Manufacturer.Contains(searchQuery)
                   || c.Model.Contains(searchQuery)
                   || c.SaleDescription.Contains(searchQuery))
                   .Count();

                return model;
            }
            else
            {
                var model = new AllCarsListModel();

                model.Cars = this._db.Cars
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Include("Images")
                    .ToList();
                model.Count = this._db.Cars.Count();

                return model;
            }
        }

        public Car FindById(int id)
                => this._db
                    .Cars
                    .Include("Images")
                    .Include(c => c.Comments)
                    .ThenInclude(a => a.Author)
                    .Where(c => c.Id == id)
                    .FirstOrDefault();

        public bool Exists(int id)
            => this._db.Cars.Any(c => c.Id == id);

        public void DeletePhoto(int photoId)
        {
            Image imageToDelete = this._db.Images.FirstOrDefault(p => p.Id == photoId);

            string pathToDelete = _hostingEnvironment.WebRootPath + "\\images\\" + imageToDelete.ImageUrl;

            File.Delete(pathToDelete);

            _db.Images.Remove(imageToDelete);
            _db.SaveChanges();
        }
    }
}
