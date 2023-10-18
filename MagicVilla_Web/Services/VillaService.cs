using MagicVilla_API;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using System;

namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _villaUrl;
        private readonly string _version;
        public VillaService(IHttpClientFactory clientFactory, ICurrentUserService currentUserService, IConfiguration configuration) : base(clientFactory, currentUserService)
        {
            _version = SD.CurrentAPIVersion;
            _clientFactory = clientFactory;
            _villaUrl = $"{configuration.GetAPIUrl()}/api/{_version}/VillaAPI";
        }

        public async Task<T> CreateAsync<T>(VillaCreateDTO dto)
        {
            return await SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Data = dto,
                Url = $"{_villaUrl}"
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.DELETE,
                Url = $"{_villaUrl}/{id}"
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.GET,
                Url = $"{_villaUrl}"
            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.GET,
                Url = $"{_villaUrl}/{id}"
            });
        }

        public async Task<T> UpdateAsync<T>(VillaUpdateDTO dto)
        {
            return await SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.PUT,
                Data = dto,
                Url = $"{_villaUrl}/{dto.Id}"
            });
        }
    }
}
