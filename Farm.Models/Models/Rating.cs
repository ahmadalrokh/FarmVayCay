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
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser? User { get; set; }
        public int? BookingId { get; set; }
        [ForeignKey("BookingId")]
        [ValidateNever]
        public Boking? Boking { get; set; }

        public int? FarmId { get; set; }
        [ForeignKey("FarmId")]
        [ValidateNever]
        public Farms? Farm { get; set; }
        public bool IsClean { get; set; }
        public bool IsFacilities { get; set; }
        public bool IsPrivacy { get; set; }
        public int stars { get; set; }
        [ValidateNever]
        public string? Comment { get; set; }
    }
}
