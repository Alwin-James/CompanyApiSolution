using CompanyApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CompanyApi.Data
{
    public class CompanyDbContext : DbContext
    {
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options)
            : base(options)
        { }

        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure that the ISIN is unique
            modelBuilder.Entity<Company>()
                .HasIndex(c => c.Isin)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
