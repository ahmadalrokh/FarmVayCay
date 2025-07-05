using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ApplicationUserRepository UserRepository { get; }
        public FarmsRepository FarmsRepository { get; }
        public FarmImagesRepository FarmImagesRepository { get; }
        public FavoriteRepository FavoriteRepository { get; }
        public RequestFarmsRepository RequestFarmsRepository { get; }
        public ApplicationUserRepository ApplicationUserRepository { get; }
        public MaintenanceRepository MaintenanceRepository { get; }
        public BokingRepository BokingRepository { get; }
        public RatingRepository RatingRepository { get; }
        public UnavailableDatesRepository UnavailableDatesRepository { get; }
        public BankRepository BankRepository { get; }
        public void Save();
    }
}
