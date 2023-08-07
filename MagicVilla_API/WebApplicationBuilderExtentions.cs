using MagicVilla_API.Repository.IRepository;
using MagicVilla_API.Repository;

namespace MagicVilla_API
{
    public static class WebApplicationBuilderExtentions
    {
        public static void RegisterDependencies(
            this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IVillaRepository, VillaRepository>();
            builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
        }
    }
}
