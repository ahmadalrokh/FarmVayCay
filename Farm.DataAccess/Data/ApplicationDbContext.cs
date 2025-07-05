using Farm.Models.Models;
using FarmVayCayTestOne.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace FarmVayCayTestOne.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<Farms> farms { get; set; }
        public DbSet<FarmImages> images { get; set; }
        public DbSet<Favorite> favorites { get; set; }
        public DbSet<RequestFarms> requestFarms { get; set; }
        public DbSet<Maintenance> maintenances { get; set; }
        public DbSet<Transportation> transportations { get; set; }
        public DbSet<Boking> bokings { get; set; }
        public DbSet<Rating> ratings { get; set; }
        public DbSet<FarmUnavailableDate> UnavailableDates { get; set; }
        public DbSet<VayCayBank> vayCayBanks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VayCayBank>().HasData(
                new VayCayBank { 
                    Id = 1, 
                    CardName = "Ali Hasan", 
                    CardNumber = "4111 2222 3333 4444",
                    ExpDate = "12/27", 
                    Balance = 10500.75 
                },
                new VayCayBank {
                    Id = 2, 
                    CardName = "Lana Hani",
                    CardNumber = "5500 1111 2222 3333",
                    ExpDate = "06/26", 
                    Balance = 8200.00 
                },
                new VayCayBank { Id = 3, CardName = "Omar Yasin", CardNumber = "3400 9999 8888 777", ExpDate = "11/28", Balance = 15750.60 },
                new VayCayBank { Id = 4, CardName = "Rana Yousef", CardNumber = "6011 0009 9011 2233", ExpDate = "09/29", Balance = 9100.10 },
                new VayCayBank { Id = 5, CardName = "Mohammad Zaid", CardNumber = "4539 5589 1234 4321", ExpDate = "01/30", Balance = 14200.00 },
                new VayCayBank { Id = 6, CardName = "Nour Ahmad", CardNumber = "3782 8224 6310 005", ExpDate = "07/27", Balance = 13200.90 },
                new VayCayBank { Id = 7, CardName = "Khaled Sami", CardNumber = "2223 4567 7890 1234", ExpDate = "05/26", Balance = 8800.40 },
                new VayCayBank { Id = 8, CardName = "Mona Saeed", CardNumber = "6011 3456 7890 1111", ExpDate = "10/28", Balance = 9700.00 },
                new VayCayBank { Id = 9, CardName = "Tariq Fares", CardNumber = "4111 6543 2109 8888", ExpDate = "03/27", Balance = 12300.00 },
                new VayCayBank { Id = 10, CardName = "Dana Ameen", CardNumber = "5105 1051 0510 5100", ExpDate = "08/29", Balance = 10000.00 },
                new VayCayBank { Id = 11, CardName = "Mohanad Salameh", CardNumber = "2000 7070 1000 1000", ExpDate = "03/29", Balance = 100000000000000000.00 },
                new VayCayBank { Id = 12, CardName = "Qusai Salameh", CardNumber = "2000 7070 1000 1000", ExpDate = "09/29", Balance = 900000000000000000.00 }                
              
                
                
                );
        }
    }
}
