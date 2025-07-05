using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Farm.DataAccess.Repository.IRepository;
using FarmVayCayTestOne.Data;

namespace Farm.DataAccess.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private  ApplicationDbContext _db;

        public ApplicationUserRepository UserRepository { get; private set; }
        public FarmsRepository FarmsRepository { get; private set; }
        public FarmImagesRepository FarmImagesRepository { get; private set; }
        public FavoriteRepository FavoriteRepository { get; private set; }
        public RequestFarmsRepository RequestFarmsRepository { get; private set; }
        public ApplicationUserRepository ApplicationUserRepository { get; private set; }
        public MaintenanceRepository MaintenanceRepository { get; private set; }
        public BokingRepository BokingRepository { get; private set; }
        public RatingRepository RatingRepository { get; private set; }
        public UnavailableDatesRepository UnavailableDatesRepository { get;private set; }
        public BankRepository BankRepository { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            UserRepository = new ApplicationUserRepository(db);
            FarmImagesRepository = new FarmImagesRepository(db);
            FarmsRepository = new FarmsRepository(db);
            FavoriteRepository = new FavoriteRepository(db);
            RequestFarmsRepository = new RequestFarmsRepository(db);
            ApplicationUserRepository = new ApplicationUserRepository(db);
            MaintenanceRepository = new MaintenanceRepository(db);
            BokingRepository= new BokingRepository(db);
            RatingRepository = new RatingRepository(db);
            UnavailableDatesRepository = new UnavailableDatesRepository(db);
            BankRepository = new BankRepository(db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
