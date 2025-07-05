using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farm.DataAccess.Repository.IRepository;
using Farm.Models.Models;
using FarmVayCayTestOne.Data;
using FarmVayCayTestOne.Models;

namespace Farm.DataAccess.Repository
{
    public class RatingRepository : Repository<Rating>, IRatingRepository
    {
        private ApplicationDbContext _db;
        public RatingRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }
        public void Update(Rating obj)
        {
            _db.ratings.Update(obj);
        }
    }
}
