using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farm.Models.Models;
using FarmVayCayTestOne.Models;

namespace Farm.Models.VM
{
    public class RatingVM
    {
        public Rating Rating { get; set; }
        
        public Farms Farm { get; set; }
        public Boking Boking { get; set; }
    }
}
