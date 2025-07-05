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
    public class Farms
    {
        [Key]
        public int Id { get; set; }
        [ValidateNever]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }


        [Required]
        public string FarmName { get; set; }
        [Required]
        public double PricePerDay { get; set; }
        [Required]
        public string Governorates { get; set; }
        [Required]
        public string LocationUrl { get; set; }
        [Required]
        public int Room { get; set; }
        [Required]
        public int PathRoom { get; set; }
        [Required]
        public int Flors { get; set; }
        [Required]
        public int Guests { get; set; }
        [Required]
        public bool SwimingPool { get; set; }
        [Required]
        public bool BarbecueArea { get; set; }
        [Required]
        public bool ChildrenArea { get; set; }
        [Required]
        public bool FootballField { get; set; }
        [Required]
        public bool BingPongTabel { get; set; }
        [Required]
        public bool BilliardoTabel { get; set; }
        [Required]
        public bool AirCondtioner { get; set; }
        [Required]
        public bool WIFI { get; set; }
        [Required]
        public string Description { get; set; }

        [ValidateNever]
        public string? Status { get; set; }
        [ValidateNever]
        public bool IsApproved { get; set; }
        [ValidateNever]
        public string ApprovalStatus { get; set; } = SD.Status_Pending;
        [ValidateNever]
        public string IsAvailable { get; set; } = SD.Status_UnAvailable;

        [ValidateNever]
        public virtual List<FarmImages>? FarmImages { get; set; }
        [ValidateNever]
        public double Rating { get; set; }
        
       
    }
}
