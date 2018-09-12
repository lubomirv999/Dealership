namespace Dealership.Models
{
    using Dealership.Data;
    using System.Collections.Generic;

    public class CarListModel
    {
        public IEnumerable<Car> Cars { get; set; }
    }
}
