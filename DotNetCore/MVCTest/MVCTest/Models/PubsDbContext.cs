using Microsoft.EntityFrameworkCore;

namespace MVCTest.Models
{
    public class PubsDbContext : DbContext
    {
        public DbSet<Author> Author { get; set; }

        public PubsDbContext(DbContextOptions<PubsDbContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);

            //optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));User Id=jdg1020;Password=7596;");
        }
    }
}
