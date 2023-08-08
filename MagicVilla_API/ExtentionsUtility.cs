using MagicVilla_API.Repository.IRepository;
using MagicVilla_API.Repository;

namespace MagicVilla_API
{
    public static class ExtentionsUtility
    {
        public static void RegisterDependencies(
            this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IVillaRepository, VillaRepository>();
            builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
        }

        public static string GetJWTSecret(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("ApiSettings:Secret");
        }
    }
}
