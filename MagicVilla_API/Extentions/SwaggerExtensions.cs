using Microsoft.OpenApi.Models;

namespace MagicVilla_API.Extentions
{
    public static class SwaggerExtensions
    {
        // Configuring customized Swagger UI.
        /// <summary>
        /// Configures Swagger for API documentation.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        public static void ConfigureSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                // Configure JWT Bearer authentication.
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    // Provide a description for using JWT Bearer authorization.
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                        "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                        "Example: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });

                // Specify security requirements for JWT Bearer.
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });

                // Configure Swagger UI for different API versions.

                // Configure Swagger UI for Version 1.
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Villa API (Version 1)",
                    Description = "API documentation for managing Villas - Version 1",
                    TermsOfService = new Uri("https://termsOfServices.testAPI.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "ContactUs",
                        Url = new Uri("https://contactus.testAPI.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "API License",
                        Url = new Uri("https://apiLicense.testAPI.com"),
                    }
                });

                // Configure Swagger UI for Version 2.
                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "Villa API (Version 2)",
                    Description = "API documentation for managing Villas - Version 2",
                    TermsOfService = new Uri("https://termsOfServices.testAPI.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "ContactUs",
                        Url = new Uri("https://contactus.testAPI.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "API License",
                        Url = new Uri("https://apiLicense.testAPI.com"),
                    }
                });
            });
        }
    }
}
