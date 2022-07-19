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
        public ICollection<Region> Regions { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
