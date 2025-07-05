using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farm.Models.Models;
using FarmVayCayTestOne.Models;

namespace Farm.DataAccess.Repository.IRepository
{
    public interface IRatingRepository : IRepository<Rating>
    {
        public void Update(Rating obj);
    }
}
