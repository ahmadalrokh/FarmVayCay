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
    public class Boking
    {
        [Key]
        public int Id { get; set; }
        public int FarmId { get; set; }
        [ForeignKey("FarmId")]
        [ValidateNever]
        public Farms? Farm { get; set; }
        public string? CustmorId { get; set; }
        [ForeignKey("CustmorId")]
        [ValidateNever]
        public ApplicationUser? Custmor { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public int ? Guests  { get; set; }

        // Transportation

        public double TotalPrice { get; set; }
        public string? PaymentStatus { get; set; } =   SD.Payment_Pending;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        [ValidateNever]
        public string BookingStatus { get; set; }
        [ValidateNever]
        public bool IsCompleted { get; set; }
        [ValidateNever]
        public bool IsRating { get; set; }
        [ValidateNever]
        public bool IsDeleted { get; set; }
    }
}
