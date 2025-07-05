using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FarmVayCayTestOne.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        [ValidateNever]
        public bool IsFarmOwner { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        [ValidateNever]
        public string PasswordRestCode { get; set; }
        [ValidateNever]
        public DateTime? PasswordRestExpiry {  get; set; }   


        [NotMapped]
        public string Role { get; set; }
    }
}
