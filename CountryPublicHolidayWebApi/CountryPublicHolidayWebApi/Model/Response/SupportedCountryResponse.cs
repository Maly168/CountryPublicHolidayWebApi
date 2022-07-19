using Newtonsoft.Json;

namespace CountryPublicHolidayWebApi.Model.Response
{
    public class SupportedCountryResponse
    {
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("regions")]
        public List<string> Regions { get; set; }

        [JsonProperty("holidayTypes")]
        public List<string> HolidayTypes { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("fromDate")]
        public CountryDateInfo FromDate { get; set; }

        [JsonProperty("toDate")]
        public CountryDateInfo ToDate { get; set; }
    }

    public class CountryDateInfo
    {
        [JsonProperty("day")]
        public int  Day { get; set; }

        [JsonProperty("month")]
        public int Month { get; set; }

        [JsonProperty("year")]
        public long Year { get; set; }
    }
}
