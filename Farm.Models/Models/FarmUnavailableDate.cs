using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Farm.Models.Models
{
    public class FarmUnavailableDate
    {
        public int Id { get; set; }
        public int FarmId { get; set; }
        [ForeignKey("FarmId")]
        [ValidateNever]
        public Farms Farm { get; set; }
        public DateTime UnavailableDateFrom { get; set; }
        public DateTime UnavailableDateTo { get; set; }
        public string? Reason {  get; set; }
    }
}
