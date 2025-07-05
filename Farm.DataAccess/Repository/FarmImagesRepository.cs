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
    public class FarmImagesRepository : Repository<FarmImages>,IFarmImagessRepository
    {
        private ApplicationDbContext _db;
        public FarmImagesRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }
        public void Update(FarmImages obj)
        {
            _db.images.Update(obj);
        }
    }
}
