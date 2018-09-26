namespace Dealership.Models.CarModels
{
    using Dealership.Data;
    using System.Collections.Generic;

    public class AllCarsListModel
    {
        public IEnumerable<Car> Cars { get; set; }

        public int Count { get; set; }
    }
}
