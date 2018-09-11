namespace Dealership.Services
{
    using Dealership.Data;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;

    public interface ICarService
    {
        IEnumerable<Car> All();

        IEnumerable<Car> Search(string searchQuery);

        void Add(Car addCarFormModel, ICollection<IFormFile> images);

        void Edit(int id, Car editCarFormModel, ICollection<IFormFile> images);

        void Delete(int IdToRemove);              

        bool Exists(int id);   

        Car FindById(int id);
    }
}
