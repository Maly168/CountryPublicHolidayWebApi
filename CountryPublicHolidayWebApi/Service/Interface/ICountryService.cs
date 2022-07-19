using CountryPublicHolidayWebApi.Model.Request;
using CountryPublicHolidayWebApi.Model.Response;

namespace CountryPublicHolidayWebApi.Service
{
    public interface ICountryService
    {
        Task<List<SupportedCountryResponse>> GetSupportedCountry();
        Task<List<GetHolidayResponse>> GetHoliday(HolidayListRequest request);
        Task<int> GetMaximumNumberOfFreeDay(HolidayListRequest request);
        Task<int> GetDayStatus(GetDayStatusRequest request);
    }
}