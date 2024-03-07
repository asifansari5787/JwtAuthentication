using JwtAuthentication.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthentication.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=JwtAuthentication; Trusted_Connection=true; TrustServerCertificate=true;");
        }

        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<User> Users => Set<User>();
    }
}
