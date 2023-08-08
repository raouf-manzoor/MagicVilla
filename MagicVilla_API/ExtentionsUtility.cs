using MagicVilla_API.Repository.IRepository;
using MagicVilla_API.Repository;
using Microsoft.IdentityModel.Tokens;

namespace MagicVilla_API
{
    public static class ExtentionsUtility
    {
        public static void RegisterDependencies(
            this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IVillaRepository, VillaRepository>();
            builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
        }

        public static string GetJWTSecret(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("ApiSettings:Secret");
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
