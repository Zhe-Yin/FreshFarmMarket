using FarmFreshMarket_201457F.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FarmFreshMarket_201457F.Data
{
    public class AuthDbContext : IdentityDbContext<User>
    {

        public DbSet<ResetPassword> ResetPasswords { get; set; }
        public DbSet<OTP> OTPS { get; set; }


        private readonly IConfiguration _configuration;
        // public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options){}
        public AuthDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("AuthConnectionString"); 
            optionsBuilder.UseSqlServer(connectionString);
        }

        //public AuthDbContext(DbContextOptions options) : base(options)
        //{
        //    Database.EnsureCreatedAsync();
        //}
    }
}
