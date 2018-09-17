namespace Dealership.Controllers
{
    using Dealership.Data;
    using Dealership.Models;
    using Dealership.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CarController : Controller
    {
        private const int PageSize = 6;

        private readonly ICarService cars;

        public CarController(ICarService cars)
        {
            this.cars = cars;
        }

        public IActionResult AllCars(string sort, string searchQuery, int page = 1, int pageSize = 6)
        {
            ViewBag.SearchQuery = searchQuery;

            return View("AllCars", new CarListModel
            {
                Cars = this.cars.All(sort, searchQuery, page, pageSize),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(this.cars.SearchCars(searchQuery).Count() / (double)PageSize),
                SearchQuery = searchQuery,
                Sort = sort
            });
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Car addCarModel, ICollection<IFormFile> images)
        {
            if (!ModelState.IsValid)
            {
                return this.View(addCarModel);
            }

            var car = new Car
            {
                Manufacturer = addCarModel.Manufacturer,
                Model = addCarModel.Model,
                YearOfProduction = addCarModel.YearOfProduction,
                BodyType = addCarModel.BodyType,
                Condition = addCarModel.Condition,
                TypeOfTransmission = addCarModel.TypeOfTransmission,
                EuroStandart = addCarModel.EuroStandart,
                EngineType = addCarModel.EngineType,
                TravelledDistance = addCarModel.TravelledDistance,
                HorsePower = addCarModel.HorsePower,
                Color = addCarModel.Color,
                SaleDescription = addCarModel.SaleDescription,
                Price = addCarModel.Price
            };

            this.cars.Add(addCarModel, images);

            TempData["message"] = "You have succesfully added your car for sale.";

            return this.RedirectToAction("AllCars");
        }

        public IActionResult Edit(int id)
        {
            var car = this.cars.FindById(id);

            if (car == null)
            {
                return NotFound();
            }

            return this.View(new Car
            {
                Id = car.Id,
                Manufacturer = car.Manufacturer,
                Model = car.Model,
                YearOfProduction = car.YearOfProduction,
                BodyType = car.BodyType,
                Condition = car.Condition,
                TypeOfTransmission = car.TypeOfTransmission,
                EuroStandart = car.EuroStandart,
                EngineType = car.EngineType,
                TravelledDistance = car.TravelledDistance,
                HorsePower = car.HorsePower,
                Color = car.Color,
                SaleDescription = car.SaleDescription,
                Price = car.Price,
                Images = car.Images
            });
        }

        [HttpPost]
        public IActionResult Edit(int id, Car editModel, ICollection<IFormFile> images)
        {
            if (!ModelState.IsValid)
            {
                return this.View(editModel);
            }

            var carExists = this.cars.Exists(id);

            if (!carExists)
            {
                return NotFound();
            }

            this.cars.Edit(id, editModel, images);

            TempData["message"] = "You have succesfully edited your car.";

            return this.RedirectToAction("Details", new { id = id });
        }

        public IActionResult Delete(int id)
        {
            this.cars.Delete(id);

            return RedirectToAction("AllCars");
        }

        public IActionResult Details(int id)
        {
            var carExists = this.cars.Exists(id);

            if (!carExists)
            {
                return NotFound();
            }

            Car car = this.cars.FindById(id);

            return View(car);
        }
        
        [HttpPost]
        public void DeletePhoto(int photoId)
        {
            this.cars.DeletePhoto(photoId);
        }
    }
}
