using appTrombinoscope.Models;
using Microsoft.EntityFrameworkCore;

namespace appTrombinoscope.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {

        }
        public DbSet<Account> Accounts { get; set; }
    }
}