using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmVayCayTestOne.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Farm.Models.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }
        
        public int? FarmId { get; set; }
        [ForeignKey("FarmId")]
        [ValidateNever]
        public Farms? Farm { get; set; }

        
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser? User { get; set; }
        


    }
}
