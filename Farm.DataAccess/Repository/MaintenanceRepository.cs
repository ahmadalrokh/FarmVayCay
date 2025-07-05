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
    public class FarmsRepository : Repository<Farms>, IFarmsRepository
    {
        private ApplicationDbContext _db;
        public FarmsRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }
        public void Update(Farms obj)
        {
            _db.farms.Update(obj);
        }
    }
}
