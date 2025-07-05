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
    public class UnavailableDatesRepository : Repository<FarmUnavailableDate>, IUnavailableDatesRepository
    {
        private ApplicationDbContext _db;
        public UnavailableDatesRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }
        public void Update(FarmUnavailableDate obj)
        {
            _db.UnavailableDates.Update(obj);
        }

    }
}
