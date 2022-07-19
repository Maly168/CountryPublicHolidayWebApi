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
            options.UseLazyLoadingProxies();
            options.UseSqlServer(Configuration.GetConnectionString("HolidayDb"), b => b.MigrationsAssembly("CountryPublicHolidayWebApi"));
            //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new SupportedCountryEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RegionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new HolidayEntityConfiguration());
   
        }
    }
}
