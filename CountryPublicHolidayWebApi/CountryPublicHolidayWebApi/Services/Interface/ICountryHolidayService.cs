using CountryPublicHolidayWebApi.Model.Request;
using CountryPublicHolidayWebApi.Model.Response;
using DataAccess.Entity;

namespace CountryPublicHolidayWebApi.Services
{
    public interface ICountryHolidayService
    {
        Task<List<SupportedCountryResponse>> GetSupportedCountry();
        Task<List<GetHolidayResponse>> GetHoliday(HolidayListRequest request);
        Task<int> GetMaximumNumberOfFreeDayAndHoliday(HolidayListRequest request);
        Task<GetDayStatusResponse> GetSpecificDayStatus(GetDayStatusRequest request);
        Task<List<Holiday>> GetSpecificDayStatusFromDB(GetDayStatusRequest request);
        Task<GetDayStatusResponse> GetSpecificDayStatusFromApi(GetDayStatusRequest request);
    }
}