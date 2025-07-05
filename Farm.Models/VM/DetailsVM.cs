using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farm.Models.Models;

namespace Farm.Models.VM
{
    public class DetailsVM
    {
        public Farms Farms { get; set; }
        public Boking Boking { get; set; }
        public List<string> ResevedDate { get; set; }
        public List<FarmImages> farmImages { get; set; } = new List<FarmImages>();
        public List<Rating>? rating { get; set; }
        public int Count { get; set; }
    }
}
