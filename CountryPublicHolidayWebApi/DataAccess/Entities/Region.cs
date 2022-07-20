using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Entity
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SupportedCountryId { get; set; }
        public virtual SupportedCountry SupportedCountry { get; set; }
        
    }
    public class RegionEntityConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasColumnName("Name").HasColumnType("NVARCHAR").HasMaxLength(50);
            builder.HasOne(p => p.SupportedCountry).WithMany(r => r.Regions).HasForeignKey(f => f.SupportedCountryId);
            
            builder.ToTable("Region");
        }
    }
}
