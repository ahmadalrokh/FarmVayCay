using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farm.Models.Models;

namespace Farm.Models.VM
{
    public class PaymentVM
    {
        public Farms Farms { get; set; }
        public VayCayBank VayCayBank { get; set; }
        public Boking Boking { get; set; }
        public List<FarmImages> FarmImages { get; set; }
    }
}
