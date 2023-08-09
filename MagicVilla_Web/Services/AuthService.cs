using MagicVilla_API;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        #region Fields
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _apiURL;
        #endregion

        #region Ctor
        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            _apiURL = configuration.GetAPIUrl();
        }
        #endregion

        #region Login
        public async Task<T> LoginAsync<T>(LoginRequestDTO obj)
        {
            return await SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Data = obj,
                Url = $"{_apiURL}/api/UsersAuthAPI/login"
            });
        }
        #endregion

        #region Register
        public async Task<T> RegisterAsync<T>(RegisterationRequestDTO obj)
        {
            return await SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Data = obj,
                Url = $"{_apiURL}/api/UsersAuthAPI/Register"
            });
        }
        #endregion

    }
}
