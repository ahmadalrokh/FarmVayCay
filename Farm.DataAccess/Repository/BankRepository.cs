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
    public class BankRepository : Repository<VayCayBank>, IBankRepository
    {
        private ApplicationDbContext _db;
        public BankRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }
        public void Update(VayCayBank obj)
        {
            _db.vayCayBanks.Update(obj);
        }
    }
}
