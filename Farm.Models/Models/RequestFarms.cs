using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farm.Utility;
using FarmVayCayTestOne.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Farm.Models.Models
{
    public class RequestFarms
    {
        [Key]
        public int Id { get; set; }
        public int FarmId { get; set; }
        [ForeignKey("FarmId")]
        [ValidateNever]
        public Farms Farm { get; set; }

        public string? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        [ValidateNever]
        public ApplicationUser? Owner { get; set; }

        public DateTime RequestDate { get; set; }

        [ValidateNever]
        public bool IsApproved { get; set; }
        
    }
}
