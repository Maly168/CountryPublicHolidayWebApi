using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Entity
{
    public class Holiday
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int DayOfWeek { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public int? RegionId { get; set; }
        public int? SupportedCountryId { get; set; }
        public virtual Region Region { get; set; }
        public virtual SupportedCountry SupportedCountry { get; set; }
    }
    public class HolidayEntityConfiguration : IEntityTypeConfiguration<Holiday>
    {
        public void Configure(EntityTypeBuilder<Holiday> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasColumnName("Name").HasColumnType("NVARCHAR").HasMaxLength(50);
            builder.Property(p => p.Type).HasColumnName("Type").HasColumnType("NVARCHAR").HasMaxLength(100);
            builder.Property(p => p.DayOfWeek).HasColumnName("DayOfWeek").HasColumnType("INT").HasMaxLength(10);
            builder.Property(p => p.Date).HasColumnName("Date").HasColumnType("NVARCHAR").HasMaxLength(50);
            builder.Property(p => p.Description).HasColumnName("Description").HasColumnType("NVARCHAR").HasMaxLength(200);
            builder.HasOne(p => p.SupportedCountry).WithOne();
            builder.HasOne(p => p.Region).WithOne();


            builder.ToTable("Holiday");
        }
    }
}
