using Newtonsoft.Json;

namespace CountryPublicHolidayWebApi.Model.Request
{
    public class HolidayListRequest
    {
        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }
    public class GetDayStatusRequest
    {
        public string Date { get; set; }
        public string Country { get; set; }
    }
}
