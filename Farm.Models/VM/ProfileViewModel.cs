using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmVayCayTestOne.Models;

namespace Farm.Models.VM
{
    public class ProfileViewModel
    {
        public ApplicationUser User { get; set; }

        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }

}
