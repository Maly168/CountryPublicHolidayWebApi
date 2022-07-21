using Newtonsoft.Json;
using System.Text;

namespace CountryPublicHolidayWebApi.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Get(string requestUrl)
        {
            var response = await _httpClient.GetStringAsync(requestUrl);
            return response;
        }

        public async Task<string> Post<T1>(T1 requestBody, string requestUrl)
        {
            try
            {
                var contentBody = JsonConvert.SerializeObject(requestBody);
                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(requestUrl, UriKind.RelativeOrAbsolute),
                    Content = new StringContent(contentBody, Encoding.UTF8, "application/json")
                };

                //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //request.Headers.TryAddWithoutValidation("Authorization", info.Authorization);
                //request.Headers.TryAddWithoutValidation("Date", info.HeaderDate);

                //request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //request.Content.Headers.TryAddWithoutValidation("Content-MD5", info.ContentMD5);

                //_logger.Info($"Request to provider: {JsonConvert.SerializeObject(request)}, request body: {JsonConvert.SerializeObject(requestBody)}");

                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    //_logger.Info($"Success: responseBody => {responseBody}");
                }
                else
                {
                    //_logger.Error(
                    //       $"[[Response] URL: {requestUrl}, StatusCode : {(int)response.StatusCode}," +
                    //       $" ResponseBody :{response.Content.ReadAsStringAsync().GetAwaiter().GetResult()}");
                }

                return responseBody;
            }
            catch (Exception ex)
            {
               // _logger.Error($"[Response] URL :: {requestUrl}, ResponseBody null or empty due to exception occurred :: {ex}");
                throw;
            }
        }
    }
}
