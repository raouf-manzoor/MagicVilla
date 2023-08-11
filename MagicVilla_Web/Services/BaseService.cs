using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        #region Fields
        public APIResponse responseModel { get; set; }
        private IHttpClientFactory _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public BaseService(IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor)
        {
            responseModel = new();
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        
        #endregion

        #region Send web request
        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                string token = _httpContextAccessor.HttpContext.Session.GetString(SD.SessionToken);

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

                // Adding token for api authorization check
                // Adding a Bearer token

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                try
                {
                    var response = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if (response.ErrorMessages?.Count > 0 || response.StatusCode == System.Net.HttpStatusCode.BadRequest ||
                        response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        response.IsSuccess = false;
                        var serializeResponse = JsonConvert.SerializeObject(response);
                        return JsonConvert.DeserializeObject<T>(serializeResponse);
                    }
                }
                catch (Exception ex)
                {
                    return JsonConvert.DeserializeObject<T>(apiContent);
                }

                return JsonConvert.DeserializeObject<T>(apiContent);
            }

            catch (Exception ex)
            {
               
                var dto = new APIResponse()
                {
                    ErrorMessages = new List<string> { ex.Message },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                return JsonConvert.DeserializeObject<T>(res);

            }
        }
        #endregion

    }
}
