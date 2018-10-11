namespace Dealership.Controllers
{
    using Dealership.Data;
    using Dealership.Models;
    using Dealership.Models.CarModels;
    using Dealership.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;

    public class CarController : Controller
    {
        private const int PageSize = 6;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICarService _carsService;
        private readonly IEmailService _emailService;        
        private readonly ICommentService _commentService;

        public CarController(UserManager<ApplicationUser> userManager, ICarService carsService, IEmailService emailService, ICommentService commentService)
        {
            this._userManager = userManager;
            this._carsService = carsService;
            this._emailService = emailService;
            this._commentService = commentService;            
        }

        public IActionResult AllCars(string sort, string searchQuery, int page = 1)
        {
            ViewBag.SearchQuery = searchQuery;
            var cars = this._carsService.All(sort, searchQuery, PageSize, page);

            return View("AllCars", new CarListModel
            {
                Cars = cars.Cars,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(cars.Count / (double)PageSize),
                SearchQuery = searchQuery,
                Sort = sort
            });
        }

        public IActionResult Buy(int id)
        {
            return View("Buy", new BuyCarFormModel { CarToBuyId = id });
        }

        [HttpPost]
        public IActionResult Buy(BuyCarFormModel PersonToSend)
        {
            if (!ModelState.IsValid)
            {
                return this.View("Buy", PersonToSend);
            }

            Car carToBuy = _carsService.FindById(PersonToSend.CarToBuyId);

            this._emailService.SendEMail(PersonToSend, carToBuy);
            return View("Thanks");
        }

        [Authorize(Roles = "Moderator")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Moderator")]
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

            this._carsService.Add(addCarModel, images);

            TempData["message"] = "You have succesfully added your car for sale.";

            return this.RedirectToAction("AllCars");
        }

        [Authorize(Roles = "Moderator")]
        public IActionResult Edit(int id)
        {
            var car = this._carsService.FindById(id);

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
        [Authorize(Roles = "Moderator")]
        public IActionResult Edit(int id, Car editModel, ICollection<IFormFile> images)
        {
            if (!ModelState.IsValid)
            {
                return this.View(editModel);
            }

            var carExists = this._carsService.Exists(id);

            if (!carExists)
            {
                return NotFound();
            }

            this._carsService.Edit(id, editModel, images);

            TempData["message"] = "You have succesfully edited your car.";

            return this.RedirectToAction("Details", new { id = id });
        }

        [Authorize(Roles = "Moderator")]
        public IActionResult Delete(int id)
        {
            this._carsService.Delete(id);

            return RedirectToAction("AllCars");
        }

        public IActionResult Details(int id)
        {
            var carExists = this._carsService.Exists(id);

            if (!carExists)
            {
                return NotFound();
            }

            Car car = this._carsService.FindById(id);

            return View(car);
        }

        [Authorize]
        public IActionResult AddComment(int carId, string content, int? parentCommentId)
        {
            var carExists = this._carsService.Exists(carId);
            ApplicationUser user = this._userManager.GetUserAsync(HttpContext.User).Result;            

            if (!carExists)
            {
                return NotFound();
            }

            if (content != null)
            {
                this._commentService.Add(carId, content, user.Id, parentCommentId);
            }            

            return RedirectToAction("Details", new { id = carId });
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public void DeleteComment(int commentId)
        {
            this._commentService.Delete(commentId);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public void DeletePhoto(int photoId)
        {
            this._carsService.DeletePhoto(photoId);
        }
    }
}
