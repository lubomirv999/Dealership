namespace Dealership.Services
{
    using Dealership.Data;
    using Dealership.Models.CarModels;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;

    public interface ICarService
    {
        AllCarsListModel All(string sort, string searchQuery, int pageSize ,int page = 1);

        AllCarsListModel SearchCars(string searchQuery, int pageSize, int page);

        Car FindById(int id);

        bool Exists(int id);

        void Add(Car addCarFormModel, ICollection<IFormFile> images);

        void Edit(int id, Car editCarFormModel, ICollection<IFormFile> images);

        void Delete(int IdToRemove);

        void DeletePhoto(int photoId);        
    }
}
