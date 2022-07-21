using CountryPublicHolidayWebApi.Exceptions;
using CountryPublicHolidayWebApi.Model.Request;
using CountryPublicHolidayWebApi.Services;
using DataAccess.DbContexts;
using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryPublicHolidayWebApi.Test.Services
{
    public class CountryHolidayServiceTest
    {
        private ICountryHolidayService _holidayService;
        private IHttpClientService _httpClientService;
        private IConfiguration _configuration;
        private DataContext _dataContext;

        [SetUp]
        public void Setup()
        {
            var inMemorySettings = new Dictionary<string, string> {
                    {"ApiUrl", "www.google.com/api"}
            };
            _configuration = new ConfigurationBuilder()
                  .AddInMemoryCollection(inMemorySettings)
                  .Build();

            _httpClientService = Substitute.For<IHttpClientService>();
            //_dataContext = Substitute.For<DataContext>();
            _holidayService = new CountryHolidayService(_httpClientService, _configuration, _dataContext);
        }
        //[Test]
        //public void GetSpecificDayStatusFromDB_day_status_should_return_null_when_not_exists_date_in_db()
        //{
        //    var mockHoliday = Substitute.For<DbSet<Holiday>>();
        //    mockHoliday.AsQueryable().Returns(null);
        //    //_dataContext.Holidays.Returns(mockHoliday);
        //    // _dataContext.Holidays.ToList().Returns(new List<Holiday> { });
        //    var actaul = _holidayService.GetSpecificDayStatusFromDB(new GetDayStatusRequest()
        //    {
        //        Country = "est",
        //        Date = "30-08-2022"
        //    });

        //    Assert.Equals(null, actaul);


        //}

        [Test]
        public void GetSpecificDayStatusFromApi_when_pass_invalid_date_format_to_api_should_throw_exception_with_Invalid_request_message()
        {
            _httpClientService.Get(Arg.Any<string>()).Returns("{error: 'Invalid request'}");
           
            Assert.That(
               () => _holidayService.GetSpecificDayStatusFromApi(new GetDayStatusRequest()
               {
                   Country = "est",
                   Date = "30.08.2022"
               }),
               Throws.Exception
                   .TypeOf<ApiErrorException>()
                   .With.Property("Error")
                   .EqualTo("Invalid request"));
        }

        [Test]
        public void GetSpecificDayStatusFromApi_when_date_request_is_working_day_should_return_true()
        {
            _httpClientService.Get(Arg.Any<string>()).Returns("{isWorkDay : true}");
            var actaul = _holidayService.GetSpecificDayStatusFromApi(new GetDayStatusRequest()
            {
                Country = "est",
                Date = "30-08-2022"
            }).GetAwaiter().GetResult();

            Assert.That(actaul.IsWorkDay, Is.EqualTo(true));
        }

        [Test]
        public void GetSpecificDayStatusFromApi_when_date_request_is_not_working_day_should_return_false()
        {
            _httpClientService.Get(Arg.Any<string>()).Returns("{isWorkDay : false}");
            var actaul = _holidayService.GetSpecificDayStatusFromApi(new GetDayStatusRequest()
            {
                Country = "est",
                Date = "30-08-2022"
            }).GetAwaiter().GetResult();

            Assert.That(actaul.IsWorkDay, Is.EqualTo(false));
        }
    }
}
