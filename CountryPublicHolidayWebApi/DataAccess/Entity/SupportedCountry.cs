using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entity
{
    public class SupportedCountry
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string HolidayTypes { get; set; }
        public virtual ICollection<Region> Regions { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class SupportedCountryEntityConfiguration : IEntityTypeConfiguration<SupportedCountry>
    {
        public void Configure(EntityTypeBuilder<SupportedCountry> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.CountryName).HasColumnName("CountryName").HasColumnType("NVARCHAR").HasMaxLength(50);
            builder.Property(p => p.CountryCode).HasColumnName("CountryCode").HasColumnType("NVARCHAR").HasMaxLength(50);
            builder.Property(p => p.HolidayTypes).HasColumnName("HolidayTypes").HasColumnType("NVARCHAR").HasMaxLength(500);
            builder.Property(p => p.FromDate).HasColumnName("FromDate").HasColumnType("NVARCHAR").HasMaxLength(100);
            builder.Property(p => p.ToDate).HasColumnName("ToDate").HasColumnType("NVARCHAR").HasMaxLength(100);
            builder.HasMany(p => p.Regions).WithOne(a => a.SupportedCountry);
            builder.ToTable("SupportedCountry");
        }
    }
}
