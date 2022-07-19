using CountryPublicHolidayWebApi.Filters;
using CountryPublicHolidayWebApi.Model.Request;
using CountryPublicHolidayWebApi.Model.Response;
using CountryPublicHolidayWebApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CountryPublicHolidayWebApi.Controllers
{
    [Route("api/[controller]")]
    //[ServiceFilter(typeof(RequestFilter))]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet("GetSupportedCountry")]
        public async Task<List<SupportedCountryResponse>> GetSupportedCountry()
        {
            return await _countryService.GetSupportedCountry();
        }

        [HttpPost("GetHoliday")]
        public async Task<List<GetHolidayResponse>> GetHoliday(HolidayListRequest request)
        {
            return await _countryService.GetHoliday(request);
        }

        [HttpPost("GetMaximumNumberOfFreeDay")]
        public async Task<int> GetMaximumNumberOfFreeDay(HolidayListRequest request)
        {
            return await _countryService.GetMaximumNumberOfFreeDay(request);
        }

        [HttpPost("GetDayStatus")]
        public async Task<int> GetDayStatus(GetDayStatusRequest request)
        {
            return await _countryService.GetDayStatus(request);
        }
    }
}
