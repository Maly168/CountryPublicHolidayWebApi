using CountryPublicHolidayWebApi.Filters;
using CountryPublicHolidayWebApi.Model.Request;
using CountryPublicHolidayWebApi.Model.Response;
using CountryPublicHolidayWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CountryPublicHolidayWebApi.Controllers
{
    [Route("api/[controller]")]
    //[ServiceFilter(typeof(RequestFilter))]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryHolidayService _countryService;

        public CountryController(ICountryHolidayService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet("GetSupportedCountry")]
        public async Task<List<SupportedCountryResponse>> GetSupportedCountry()
        {
            return await _countryService.GetSupportedCountry();
        }

        [HttpPost("GetHoliday")]
        [ServiceFilter(typeof(RequestFilter))]
        public async Task<List<GetHolidayResponse>> GetHoliday(HolidayListRequest request)
        {
            return await _countryService.GetHoliday(request);
        }

        [HttpPost("GetMaximumNumberOfFreeDay")]
        public async Task<int> GetMaximumNumberOfFreeDay(HolidayListRequest request)
        {
            return await _countryService.GetMaximumNumberOfFreeDayAndHoliday(request);
        }

        [HttpPost("GetDayStatus")]
        public async Task<GetDayStatusResponse> GetDayStatus(GetDayStatusRequest request)
        {
            return await _countryService.GetSpecificDayStatus(request);
        }
    }
}
