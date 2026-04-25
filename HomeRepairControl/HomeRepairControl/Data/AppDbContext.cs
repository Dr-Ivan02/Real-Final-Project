using HomeRepairControl.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeRepairControl.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<RepairItem> RepairItems { get; set; }
        public DbSet<RepairNote> RepairNotes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.\\SQLEXPRESS;Database=HomeRepairDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}