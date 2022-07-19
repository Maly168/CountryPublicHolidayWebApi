using CountryPublicHolidayWebApi.Exceptions;
using CountryPublicHolidayWebApi.Model.Request;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace CountryPublicHolidayWebApi.Filters
{
    public class RequestFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.ActionArguments.Values.FirstOrDefault() as HolidayListRequest;
            if (!IsValidateRequest(request))
            {
                throw new HolidayExceptionFilter(404, "Bad Request");
            };

        }

        private bool IsValidateRequest(HolidayListRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Country)
                || string.IsNullOrEmpty(request.Year.ToString())
                || request.Year == 0)
            { 
                return false;
            }
            return true;
        }
    }
}
