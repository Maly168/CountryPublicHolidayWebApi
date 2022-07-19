namespace CountryPublicHolidayWebApi.Exceptions
{
    public class HolidayExceptionFilter : Exception
    {
        public int ErrorCode { get; set; }
        public string Messages { get; set; }
        public HolidayExceptionFilter(int errorCode, string message)
        {
            ErrorCode = errorCode;
            Messages = message;
        }
    }
}
