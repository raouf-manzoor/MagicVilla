using MagicVilla_Web.Services.IServices;
using MagicVilla_Web.Services;

namespace MagicVilla_API
{
    public static class ExtentionsUtility
    {
        public static void RegisterDependencies(
            this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient<IVillaService, VillaService>();
            builder.Services.AddScoped<IVillaService, VillaService>();

            builder.Services.AddHttpClient<IVillaNumberService, VillaNumberService>();
            builder.Services.AddScoped<IVillaNumberService, VillaNumberService>();

            builder.Services.AddHttpClient<IAuthService, AuthService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
        }

        public static string GetAPIUrl(this IConfiguration configuration) { 

            return configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }
    }
}
