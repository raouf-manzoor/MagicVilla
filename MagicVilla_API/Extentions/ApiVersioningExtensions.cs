using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Extentions
{
    public static class ApiVersioningExtensions
    {
        // Configuring Version settings to allow multiple versions

        /// <summary>
        /// Configures support for API versioning in the application.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        public static void ConfigureApiVersionSupport(this WebApplicationBuilder builder)
        {
            // Add API versioning services to the application.
            builder.Services.AddApiVersioning(options =>
            {
                // Assume the default version when the client does not specify a version.
                options.AssumeDefaultVersionWhenUnspecified = true;

                // Set the default API version to Major version 1.0.
                options.DefaultApiVersion = new ApiVersion(1, 0);

                // Include supported API versions in the response object.
                options.ReportApiVersions = true;
            });

            // Add versioned API explorer services to the application.
            builder.Services.AddVersionedApiExplorer(options =>
            {
                // Format API version groups as 'v{version}' (e.g., 'v1', 'v2').
                options.GroupNameFormat = "'v'VVV";

                // Automatically substitute the version number in URLs.
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }
}
