namespace Dealership.Controllers
{
    using Dealership.Models;
    using Dealership.Models.CarViewModels;
    using Dealership.Services;
    using Microsoft.AspNetCore.Mvc;

    public class CarController : Controller
    {
        private readonly ICarService cars;

        public CarController(ICarService cars)
        {
            this.cars = cars;
        }

        public ViewResult AllCars()
        {
            return View(new CarListModel
            {
                Cars = this.cars.All()
            });
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

            TempData["message"] = "You have succesfully added your car for sale.";

            return this.RedirectToAction(nameof(AllCars));
        }        

        public IActionResult Edit(int id)
        {
            var car = this.cars.FindById(id);

            if (car == null)
            {
                return NotFound();
            }

            string images = "";

            foreach (var image in car.Images)
            {
                images += image.ImageUrl + ", ";
            }
            
            return this.View(new CarCreateFormModel
            {
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
                Images = images.Trim(new char[] { ' ', ',' })
            });
        }

        [HttpPost]
        public IActionResult Edit(int id, CarCreateFormModel editModel)
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

            this.cars.Edit(id, editModel.Manufacturer, editModel.Model, editModel.YearOfProduction, editModel.BodyType, editModel.Condition, editModel.TypeOfTransmission, editModel.EuroStandart, editModel.EngineType, editModel.TravelledDistance, editModel.HorsePower, editModel.Color, editModel.SaleDescription, editModel.Price,
                editModel.Images);

            TempData["message"] = "You have succesfully edited your car.";

            return this.RedirectToAction(nameof(AllCars));
        }

        [HttpPost]
        public IActionResult Delete(int carId)
        {
            if (this.cars.Delete(carId))
            {
                TempData["message"] = "You have successfully deleted your car!";
            }
            else
            {
                TempData["message"] = "You failed deleting your car!";
            }

            return RedirectToAction("AllCars");
        }
    }
}
