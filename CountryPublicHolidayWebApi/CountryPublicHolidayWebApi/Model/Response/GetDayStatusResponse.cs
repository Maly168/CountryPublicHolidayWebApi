using Newtonsoft.Json;

namespace CountryPublicHolidayWebApi.Model.Response
{
    public class GetDayStatusResponse 
    {
        [JsonProperty("isWorkDay")]
        public bool IsWorkDay { get; set; }
    }
}
