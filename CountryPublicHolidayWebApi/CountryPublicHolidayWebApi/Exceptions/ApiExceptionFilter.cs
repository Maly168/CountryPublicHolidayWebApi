using Newtonsoft.Json;

namespace CountryPublicHolidayWebApi.Exceptions
{
    public class ApiExceptionFilter : Exception
    {
        public int ErrorCode { get; set; }
        public string Messages { get; set; }
        public ApiExceptionFilter(int errorCode, string message)
        {
            ErrorCode = errorCode;
            Messages = message;
        }
    }

    public class ApiErrorException : Exception
    {
        [JsonProperty("error")]
        public string Error { get; set; }
        public ApiErrorException(string errorMessages)
        {
           Error = errorMessages;
        }
    }
}
