using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        private IHttpClientFactory _httpClient; 
        public BaseService(IHttpClientFactory httpClient)
        {
            responseModel = new();
            _httpClient = httpClient;

        }
        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try {
                var client = _httpClient.CreateClient("MagicAPI");

                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);

                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }

                switch (apiRequest.APIType)
                {

                    case SD.APIType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.APIType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.APIType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;

                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(apiContent);
            }

            catch(Exception ex) {

                var dto = new APIResponse()
                {
                    ErrorMessages = new List<string> { ex.Message },
                    IsSuccess = false
                };
                var res=JsonConvert.SerializeObject(dto);   
                return JsonConvert.DeserializeObject<T>(res);

            }
        }
    }
}
