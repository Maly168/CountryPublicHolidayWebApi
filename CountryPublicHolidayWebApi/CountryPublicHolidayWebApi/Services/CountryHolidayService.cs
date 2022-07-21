using CountryPublicHolidayWebApi.Exceptions;
using CountryPublicHolidayWebApi.Model.Request;
using CountryPublicHolidayWebApi.Model.Response;
using DataAccess.DbContexts;
using DataAccess.Entity;
using Newtonsoft.Json;
using System.Data.Entity;

namespace CountryPublicHolidayWebApi.Services
{
    public class CountryHolidayService : ICountryHolidayService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _apiUrl;
        private DataContext _dataContext;

        public CountryHolidayService(IHttpClientService httpClientService,
            IConfiguration configuration, DataContext dataContext)
        {
            _httpClientService = httpClientService;
            _apiUrl = configuration.GetValue<string>("ApiUrl");
            _dataContext = dataContext;
        }

        public async Task<GetDayStatusResponse> GetSpecificDayStatus(GetDayStatusRequest request)
        {
            var getSpecificDayStatus = await GetSpecificDayStatusFromDB(request);
            if (getSpecificDayStatus.Count > 0)
            {
                return new GetDayStatusResponse()
                {
                    IsWorkDay = true
                };
            }
            return await GetSpecificDayStatusFromApi(request);
        }
        public async Task<List<SupportedCountryResponse>> GetSupportedCountry()
        {
            var countries = await GetSuportedCountryFromDb();
            if (countries.Count > 0)
            {
                return countries;
            }
            return await GetSupportedCountryByApi();
        }

        public async Task<List<GetHolidayResponse>> GetHoliday(HolidayListRequest request)
        {
            var getHolidayDataFromDb = await GetHolidayFromDB(request);
            if (getHolidayDataFromDb.Count > 0)
            {
                return getHolidayDataFromDb;
            }

            return await GetHolidayFromApi(request);
        }

        public async Task<int> GetMaximumNumberOfFreeDayAndHoliday(HolidayListRequest request)
        {
            var numberOfFreeDayAndHoliday = await GetMaximumNumberOfFreeDayAndHolidayFromDB(request);
            if (numberOfFreeDayAndHoliday > 0)
            {
                return numberOfFreeDayAndHoliday;
            }

            return await GetMaximumNumberOfFreeDayAndHolidayFromApi(request);
        }

        public async Task<GetDayStatusResponse> GetSpecificDayStatusFromApi(GetDayStatusRequest request)
        {
            var url = $"{_apiUrl}action=isWorkDay&date={request.Date}&country={request.Country}";
            var apiResponse = await _httpClientService.Get(url);

            if (apiResponse.Contains("error"))
            {
                var errorMessage = JsonConvert.DeserializeObject<ApiErrorResponse>(apiResponse);
                throw new ApiErrorException(errorMessage.Error);
            }
            var dayStatus = JsonConvert.DeserializeObject<GetDayStatusResponse>(apiResponse);

            return dayStatus;
        }

        public async Task<List<Holiday>> GetSpecificDayStatusFromDB(GetDayStatusRequest request)
        {
            var dayStatus = _dataContext.Holidays.Where(h => h.Date == request.Date
                             && h.SearchCountry == request.Country).ToList();
            return dayStatus;
        }

        private async Task<int> GetMaximumNumberOfFreeDayAndHolidayFromDB(HolidayListRequest request)
        {
            var holidays = _dataContext.Holidays.Where(h => h.SearchCountry == request.Country.ToLower()
                            && h.SearchYear == request.Year).ToList();

            return holidays.Count();
        }

        private async Task<int> GetMaximumNumberOfFreeDayAndHolidayFromApi(HolidayListRequest request)
        {
            var url = $"{_apiUrl}action=getHolidaysForYear&year={request.Year}&country={request.Country}&holidayType=all";
            var apiResponse = await _httpClientService.Get(url);
            if (apiResponse.Contains("error"))
            {
                var errorMessage = JsonConvert.DeserializeObject<ApiErrorResponse>(apiResponse);
                throw new ApiErrorException(errorMessage.Error);
            }
            if (apiResponse.Contains("error"))
            {
                var errorMessage = JsonConvert.DeserializeObject<ApiErrorResponse>(apiResponse);
                throw new ApiErrorException(errorMessage.Error);
            }
            var response = JsonConvert.DeserializeObject<List<HolidayListResponse>>(apiResponse);
            StoreHolidayIntoDb(response, request);
            return response.Count;
        }

        private async Task<List<SupportedCountryResponse>> GetSupportedCountryByApi()
        {
            var url = $"{_apiUrl}action=getSupportedCountries";
            var apiResponse = await _httpClientService.Get(url);
            if (apiResponse.Contains("error"))
            {
                var errorMessage = JsonConvert.DeserializeObject<ApiErrorResponse>(apiResponse);
                throw new ApiErrorException(errorMessage.Error);
            }
            var supportedCountries = JsonConvert.DeserializeObject<List<SupportedCountryResponse>>(apiResponse);
            StoreSupportedCountry(supportedCountries);

            return supportedCountries;
        }

        private async Task<List<SupportedCountryResponse>> GetSuportedCountryFromDb()
        {
            var countries = _dataContext.SupportedCountry
                               .Include(x => x.Regions).ToList();
            var response = new List<SupportedCountryResponse>();
            foreach (var country in countries)
            {
                var regions = new List<string>();
                if (country.Regions != null)
                {
                    foreach (var region in country.Regions)
                    {
                        regions.Add(region.Name);
                    }
                }

                response.Add(new SupportedCountryResponse()
                {
                    CountryCode = country.CountryCode,
                    FullName = country.CountryName,
                    Regions = regions,
                    HolidayTypes = JsonConvert.DeserializeObject<List<string>>(country.HolidayTypes),
                    FromDate = new CountryDateInfo()
                    {
                        Day = int.Parse(country.FromDate.Split('-')[0]),
                        Month = int.Parse(country.FromDate.Split('-')[1]),
                        Year = int.Parse(country.FromDate.Split('-')[2]),
                    },
                    ToDate = new CountryDateInfo()
                    {
                        Day = int.Parse(country.FromDate.Split('-')[0]),
                        Month = int.Parse(country.FromDate.Split('-')[1]),
                        Year = int.Parse(country.FromDate.Split('-')[2]),
                    }
                });
            }
            return response;
        }

        private async Task<List<GetHolidayResponse>> GetHolidayFromDB(HolidayListRequest request)
        {
            var holidayResponse = new List<HolidayListResponse>();
            var apiResponse = new List<GetHolidayResponse>();
            var holidays = _dataContext.Holidays.Where(h => h.SearchCountry == request.Country.ToLower()
                            && h.SearchYear == request.Year).ToList();

            foreach (var holiday in holidays)
            {
                holidayResponse.Add(new HolidayListResponse()
                {
                    Date = new HolidayDateInfo()
                    {
                        Day = int.Parse(holiday.Date.Split('-')[0]),
                        Month = int.Parse(holiday.Date.Split('-')[1]),
                        Year = int.Parse(holiday.Date.Split('-')[2]),
                    },
                    HolidayType = holiday.Type,
                    Name = JsonConvert.DeserializeObject<List<HolidayNameInfo>>(holiday.Name)
                });
            }

            var result = holidayResponse.GroupBy(u => u.Date.Month);
            foreach (var resultHoliday in result)
            {
                var holidayObj = new GetHolidayResponse()
                {
                    Month = GetMonth(resultHoliday.Key),
                    HolidayInfo = new List<HolidayListResponse>()
                };
                foreach (var holiday in resultHoliday)
                {
                    holidayObj.HolidayInfo.Add(holiday);
                };

                apiResponse.Add(holidayObj);
            }

            return apiResponse;
        }

        private async Task<List<GetHolidayResponse>> GetHolidayFromApi(HolidayListRequest request)
        {
            var url = $"{_apiUrl}action=getHolidaysForYear&year={request.Year}&country={request.Country}&holidayType=all";
            var apiResponse = await _httpClientService.Get(url);
            if (apiResponse.Contains("error"))
            {
                var errorMessage = JsonConvert.DeserializeObject<ApiErrorResponse>(apiResponse);
                throw new ApiErrorException(errorMessage.Error);
            }
            var holidayInfo = JsonConvert.DeserializeObject<List<HolidayListResponse>>(apiResponse);
           
            StoreHolidayIntoDb(holidayInfo, request);
            var response = new List<GetHolidayResponse>();
            var result = holidayInfo.GroupBy(u => u.Date.Month);
            foreach (var subHoliday in result)
            {
                var holidayObj = new GetHolidayResponse()
                {
                    Month = GetMonth(subHoliday.Key),
                    HolidayInfo = new List<HolidayListResponse>()
                };
                foreach (var holiday in subHoliday)
                {
                    holidayObj.HolidayInfo.Add(holiday);
                };

                response.Add(holidayObj);
            }

            return response;
        }

        private void StoreSupportedCountry(List<SupportedCountryResponse> countries)
        {
            foreach (var country in countries)
            {
                var supportedCountry = new SupportedCountry()
                {
                    CountryCode = country.CountryCode,
                    CountryName = country.FullName,
                    HolidayTypes = JsonConvert.SerializeObject(country.HolidayTypes),
                    FromDate = $"{country.FromDate.Day}-{country.FromDate.Month}-{country.FromDate.Year}",
                    ToDate = $"{country.ToDate.Day}-{country.ToDate.Month}-{country.ToDate.Year}",
                    Regions = new List<Region>()
                };

                foreach (var region in country.Regions)
                {
                    supportedCountry.Regions.Add(new Region()
                    {
                        Name = region,
                        SupportedCountry = supportedCountry
                    });
                }

                _dataContext.Add(supportedCountry);
            }
            _dataContext.SaveChanges();
        }

        private void StoreHolidayIntoDb(List<HolidayListResponse> holidayInfos, HolidayListRequest request)
        {
            foreach (var holiday in holidayInfos)
            {
                var holidayData = new Holiday()
                {
                    SearchCountry = request.Country,
                    SearchYear = request.Year,
                    Name = JsonConvert.SerializeObject(holiday.Name),
                    Date = $"{holiday.Date.Day}-{holiday.Date.Month}-{holiday.Date.Year}",
                    DayOfWeek = holiday.Date.DayOfWeek,
                    Type = holiday.HolidayType,
                    Description = string.Empty
                };
                _dataContext.Add(holidayData);
            }
            _dataContext.SaveChanges();
        }

        private string GetMonth(int month)
        {
            return month switch
            {
                1 => "January",
                2 => "Febuary",
                3 => "March",
                4 => "April",
                5 => "May",
                6 => "Jun",
                7 => "July",
                8 => "August",
                9 => "September",
                10 => "October",
                11 => "November",
                12 => "December",
                _ => "Unknown",
            };
        }
    }
}