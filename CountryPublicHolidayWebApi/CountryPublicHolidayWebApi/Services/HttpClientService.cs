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

               
                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    //Write log here
                    //_logger.Info($"Success: responseBody => {responseBody}");
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
