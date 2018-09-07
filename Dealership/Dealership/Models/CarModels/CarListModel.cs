using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dealership.Data;

namespace Dealership.Models
{
    public class CarListModel
    {
        public IEnumerable<Car> Cars { get; set; }
    }
}
