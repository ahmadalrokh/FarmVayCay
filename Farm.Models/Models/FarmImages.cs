using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Farm.Models.Models
{
    public class FarmImages
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; }

        public int FarmId { get; set; }
        [ForeignKey("FarmId")]
        [ValidateNever]
        public Farms Farm { get; set; }
    }
}
