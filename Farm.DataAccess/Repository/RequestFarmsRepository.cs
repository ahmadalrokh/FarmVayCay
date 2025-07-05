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
    public class RequestFarmsRepository : Repository<RequestFarms>, IRequestFarmsRepository
    {
        private ApplicationDbContext _db;
        public RequestFarmsRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }
        public void Update(RequestFarms obj)
        {
            _db.requestFarms.Update(obj);
        }
    }
}
