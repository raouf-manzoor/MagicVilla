using MagicVilla_Web.Services.IServices;
using MagicVilla_Web.Services;

namespace MagicVilla_API
{
    public static class WebApplicationBuilderExtentions
    {
        public static void RegisterDependencies(
            this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient<IVillaService, VillaService>();
            builder.Services.AddScoped<IVillaService, VillaService>();

            builder.Services.AddHttpClient<IVillaNumberService, VillaNumberService>();
            builder.Services.AddScoped<IVillaNumberService, VillaNumberService>();
        }
    }
}
