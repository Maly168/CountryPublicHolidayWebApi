using CountryPublicHolidayWebApi.Model.Request;
using CountryPublicHolidayWebApi.Model.Response;
using DataAccess.DbContexts;
using DataAccess.Entity;
using Newtonsoft.Json;
using System.Data.Entity;

namespace CountryPublicHolidayWebApi.Service
{
    public class CountryService : ICountryService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _apiUrl;
        private DataContext _dataContext;
        public CountryService(IHttpClientService httpClientService,
            IConfiguration configuration, DataContext dataContext)
        {
            _httpClientService = httpClientService;
            _apiUrl = configuration.GetValue<string>("ApiUrl");
            _dataContext = dataContext;
        }

        public Task<int> GetDayStatus(GetDayStatusRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GetHolidayResponse>> GetHoliday(HolidayListRequest request)
        {
            var url = $"{_apiUrl}action=getHolidaysForYear&year={request.Year}&country={request.Country}&holidayType=all";
            var apiResponse = await _httpClientService.Get(url);
            var holidayInfo = JsonConvert.DeserializeObject<List<HolidayListResponse>>(apiResponse);
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

        public async Task<int> GetMaximumNumberOfFreeDay(HolidayListRequest request)
        {
            var url = $"{_apiUrl}action=getHolidaysForYear&year={request.Year}&country={request.Country}&holidayType=all";
            var apiResponse = await _httpClientService.Get(url);
            var response = JsonConvert.DeserializeObject<List<HolidayListResponse>>(apiResponse);
            return response.Count;
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

        private async Task<List<SupportedCountryResponse>> GetSupportedCountryByApi()
        {
            var url = $"{_apiUrl}action=getSupportedCountries";
            var apiResponse = await _httpClientService.Get(url);
            var supportedCountries = JsonConvert.DeserializeObject<List<SupportedCountryResponse>>(apiResponse);
            StoreSupportedCountry(supportedCountries);

            return supportedCountries;
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
        private async Task<List<SupportedCountryResponse>> GetSuportedCountryFromDb()
        { 
            var countries =  _dataContext.SupportedCountry
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
                }) ;
               
            }
            return response;
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
