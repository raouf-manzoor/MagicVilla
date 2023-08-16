using MagicVilla_Utility;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class CurrentUserService : ICurrentUserService
    {

        #region Fields
       
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        #endregion

        #region Ctor
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Get Token
        public string? Token => _httpContextAccessor.HttpContext.Session.GetString(SD.SessionToken);

        #endregion

        #region Check Login status
        public bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(Token);
        }

        #endregion
    }
}
