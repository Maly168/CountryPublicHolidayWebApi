using Newtonsoft.Json;

namespace CountryPublicHolidayWebApi.Model.Response
{
    public class HolidayListResponse 
    {
        [JsonProperty("date")]
        public HolidayDateInfo Date { get; set; }

        [JsonProperty("name")]
        public List<HolidayNameInfo> Name { get; set; }

        [JsonProperty("holidayType")]
        public string HolidayType { get; set; }
    }

    public class HolidayNameInfo
    {
        [JsonProperty("lang")]
        public string Language { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class HolidayDateInfo : CountryDateInfo
    {
        [JsonProperty("dayOfWeek")]
        public int DayOfWeek { get; set; }
    }

    public class GetHolidayResponse
    {
        public string Month { get; set; }
        public List<HolidayListResponse> HolidayInfo { get; set; }
    }
    public class ApiErrorResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
