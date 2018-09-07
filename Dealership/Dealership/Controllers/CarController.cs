namespace Dealership.Controllers
{
    using Dealership.Models;
    using Dealership.Models.CarViewModels;
    using Dealership.Data;
    using Dealership.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;


    public class CarController : Controller
    {
        private readonly ICarService cars;

        public CarController(ICarService cars)
        {
            this.cars = cars;
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(CarCreateFormModel addCarFormModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(addCarFormModel);
            }

            var success = this.cars.Add(
                addCarFormModel.Manufacturer,
                addCarFormModel.Model,
                addCarFormModel.YearOfProduction,
                addCarFormModel.BodyType,
                addCarFormModel.Condition,
                addCarFormModel.TypeOfTransmission,
                addCarFormModel.EuroStandart,
                addCarFormModel.EngineType,
                addCarFormModel.TravelledDistance,
                addCarFormModel.HorsePower,
                addCarFormModel.Color,
                addCarFormModel.SaleDescription,
                addCarFormModel.Price,
                addCarFormModel.Images);

            if (!success)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction(nameof(Create));
        }

        public ViewResult AllCars()
        {
            return View(new CarListModel
            {
                Cars = from car in cars.Cars()
                       from img in cars.Images()
                       where car.Id == img.CarId
                       select car
            }
                );
        }
        [HttpPost]
        public IActionResult Delete(int carId)
        {
            if (cars.Delete(carId))
            {
                TempData["message"] = "Success!";
            }
            else
            {
                TempData["message"] = "Failed!";
            }

            return RedirectToAction("AllCars");
        }
    }
}
