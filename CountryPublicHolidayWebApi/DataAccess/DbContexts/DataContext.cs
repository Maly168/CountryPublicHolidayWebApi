using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.DbContexts
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public DbSet<SupportedCountry> SupportedCountry { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlServer(Configuration.GetConnectionString("HolidayDb"), b => b.MigrationsAssembly("CountryPublicHolidayWebApi"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupportedCountry>()
               .HasMany(c => c.Regions).WithOne(x => x.SupportedCountry);
            //.HasForeignKey(x => x.CategoryId); ;

            //modelBuilder.Entity<Region>()
            //    .HasOne(c => c.SupportedCountry)
            //    .WithOne().HasForeignKey(x => x.);

            base.OnModelCreating(modelBuilder);
        }
    }
}
