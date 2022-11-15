using DIgitalCard.Lib.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace DigitalCard.Api.Data
{
  
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {
              


            }


        public DbSet<Card> Cards { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CardStatusRequest> CardStatusRequests { get; set; }

          

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
              //  new DbInitializer(modelBuilder).Seed();
            }

    

        }


    
}
