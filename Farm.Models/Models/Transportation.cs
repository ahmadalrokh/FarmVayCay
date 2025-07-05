using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm.Models.Models
{
    public class Transportation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string VhicleId { get; set; }
        [Required]
        public string VehicleType { get; set; }
        [Required]
        public string Location { get; set; }

    }
}
