namespace Dealership.Services
{
    using Dealership.Data;
    using Dealership.Models.CarModels;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;

    public interface ICarService
    {
        AllCarsListModel All(string sort, string searchQuery, int pageSize ,int page = 1);        

        void Add(Car addCarFormModel, ICollection<IFormFile> images);

        void Edit(int id, Car editCarFormModel, ICollection<IFormFile> images);

        void Delete(int IdToRemove);

        void DeletePhoto(int photoId);

        bool Exists(int id);   

        Car FindById(int id);

        AllCarsListModel SearchCars(string searchQuery, int pageSize, int page);
    }
}
