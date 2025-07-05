using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farm.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Farm.Models.VM
{
    public class FarmVM
    {
        [ValidateNever]
        public Farms Farms { get; set; } = new Farms();
        [ValidateNever]
        public  List<FarmImages> farmImages { get; set; }=new List<FarmImages>();
        public Boking? boking { get; set; } = new Boking();
        public List<Rating>? rating { get; set; }
       
    }
}
