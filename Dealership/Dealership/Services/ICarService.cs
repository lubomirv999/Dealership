namespace Dealership.Services
{
    using Dealership.Data;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;

    public interface ICarService
    {
        IEnumerable<Car> All(string sort, int page = 1, int pageSize = 6);

        IEnumerable<Car> Search(string searchQuery, int page = 1, int pageSize = 6);

        void Add(Car addCarFormModel, ICollection<IFormFile> images);

        void Edit(int id, Car editCarFormModel, ICollection<IFormFile> images);

        void Delete(int IdToRemove);

        void DeletePhoto(int photoId);

        bool Exists(int id);   

        Car FindById(int id);

        int Total();
    }
}
