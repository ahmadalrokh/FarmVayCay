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
    public class BokingRepository : Repository<Boking>, IBokingRepository
    {
        private ApplicationDbContext _db;
        public BokingRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }
        public void Update(Boking obj)
        {
            _db.bokings.Update(obj);
        }
    }
}
