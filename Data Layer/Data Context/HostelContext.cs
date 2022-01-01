using Data_Layer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Data_Context
{
    public class HostelContext : DbContext
    {
        public HostelContext(){}

        public HostelContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Orders> orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Categories> categories { get; set; }

        public DbSet<Products> products { get; set; }

        public DbSet<Users> users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=YOGA-X1;Database=HostelCrustDb;Trusted_Connection=True;");
            }
        }
    }
}
