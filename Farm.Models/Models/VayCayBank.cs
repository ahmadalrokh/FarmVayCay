using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm.Models.Models
{
    public class VayCayBank
    {
        public int Id { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpDate { get; set; }
        public double Balance { get; set; }
    }
}
