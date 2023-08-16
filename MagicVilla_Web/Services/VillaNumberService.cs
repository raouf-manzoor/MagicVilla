using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _villaUrl;
        public VillaNumberService(IHttpClientFactory clientFactory, IConfiguration configuration,ICurrentUserService currentUserService) : base(clientFactory, currentUserService)
        {
            _clientFactory = clientFactory;
            _villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public async Task<T> CreateAsync<T>(VillaNumberCreateDTO dto)
        {
            return await SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Data = dto,
                Url = $"{_villaUrl}/api/VillaNumberAPI"
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.DELETE,
                Url = $"{_villaUrl}/api/VillaNumberAPI/{id}"
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.GET,
                Url = $"{_villaUrl}/api/VillaNumberAPI"
            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.GET,
                Url = $"{_villaUrl}/api/VillaNumberAPI/{id}"
            });
        }

        public async Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto)
        {
            return await SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.PUT,
                Data = dto,
                Url = $"{_villaUrl}/api/VillaNumberAPI/{dto.VillaNo}"
            });
        }
    }
}
