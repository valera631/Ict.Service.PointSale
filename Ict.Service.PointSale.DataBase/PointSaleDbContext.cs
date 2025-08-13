using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ict.Service.PointSale.DataBase.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Ict.Service.PointSale.DataBase
{
    public class PointSaleDbContext : DbContext
    {
        public DbSet<PointSaleEntity> PointSaleEntities { get; set; }
        public DbSet<PointSaleActivity> PointSaleActivities { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<OrganizationType> OrganizationTypes { get; set; }
        public DbSet<ClosingStatus> ClosingStatuses { get; set; }
        public DbSet<CreationType> CreationTypes { get; set; }
        public DbSet<Chief> Chiefs { get; set; }
        public DbSet<ChiefPosition> ChiefPositions{ get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Logo> Logos { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<OwnerType> OwnerTypes { get; set; }
        public DbSet<PointSaleSchedule> PointSaleSchedules { get; set; }
        public DbSet<PendingVerification> PendingVerifications { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<CategoryPointSale> CategoryPointSales{ get; set; }
        public DbSet<AlternativeWord> AlternativeWords { get; set; }



        public PointSaleDbContext(DbContextOptions<PointSaleDbContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }


    }
}
