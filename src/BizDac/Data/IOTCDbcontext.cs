using Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class IOTCDbcontext : DbContext
    {
        public IOTCDbcontext(DbContextOptions<IOTCDbcontext> options) : base(options)
        {
        }

        public DbSet<Seller> Seller { get; set; }
        public DbSet<Store> Store { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
