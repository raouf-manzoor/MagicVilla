//using MagicVilla_API.Repository.IRepository;
//using MagicVilla_API.Repository;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using System.Text;
//using Microsoft.OpenApi.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Identity;
//using MagicVilla_API.Data;
//using MagicVilla_API.Models;

//namespace MagicVilla_API
//{
//    public static class ExtentionsUtility
//    {
//        // Registering all the services with respective interfaces
//        /// <summary>
//        /// Registers application dependencies for dependency injection.
//        /// </summary>
//        /// <param name="builder">The web application builder.</param>
//        public static void RegisterDependencies(this WebApplicationBuilder builder)
//        {
//            // Register repository for managing villas.
//            builder.Services.AddScoped<IVillaRepository, VillaRepository>();

//            // Register repository for managing villa numbers.
//            builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();

//            // Register repository for managing users.
//            builder.Services.AddScoped<IUserRepository, UserRepository>();
//        }


//        // Configuring Authentication and JWT setup.
//        /// <summary>
//        /// Configures JWT authentication for the application.
//        /// </summary>
//        /// <param name="builder">The web application builder.</param>
//        public static void ConfigureJwtAuthentication(this WebApplicationBuilder builder)
//        {
//            // Configure authentication services for JWT.
//            var authenticationBuilder = builder.Services.AddAuthentication(options =>
//            {
//                // Use JWT Bearer as the default authentication scheme for both authentication and challenge.
//                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//            });

//            // Configure JWT Bearer authentication.
//            authenticationBuilder.AddJwtBearer(x =>
//            {
//                // Require HTTPS metadata and save tokens.
//                x.RequireHttpsMetadata = false;
//                x.SaveToken = true;

//                // Set token validation parameters.
//                x.TokenValidationParameters = new()
//                {
//                    // Validate the issuer signing key.
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.RetrieveJwtSecret())),

//                    // Disable issuer and audience validation for flexibility.
//                    ValidateIssuer = false,
//                    ValidateAudience = false
//                };
//            });
//        }


//        // Configuring customized Swagger UI.
//        /// <summary>
//        /// Configures Swagger for API documentation.
//        /// </summary>
//        /// <param name="builder">The web application builder.</param>
//        public static void ConfigureSwagger(this WebApplicationBuilder builder)
//        {
//            builder.Services.AddSwaggerGen(options =>
//            {
//                // Configure JWT Bearer authentication.
//                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//                {
//                    // Provide a description for using JWT Bearer authorization.
//                    Description =
//                        "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
//                        "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
//                        "Example: \"Bearer 12345abcdef\"",
//                    Name = "Authorization",
//                    In = ParameterLocation.Header,
//                    Scheme = "Bearer"
//                });

//                // Specify security requirements for JWT Bearer.
//                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
//        {
//            {
//                new OpenApiSecurityScheme
//                {
//                    Reference = new OpenApiReference
//                    {
//                        Type = ReferenceType.SecurityScheme,
//                        Id = "Bearer"
//                    },
//                    Scheme = "oauth2",
//                    Name = "Bearer",
//                    In = ParameterLocation.Header
//                },
//                new List<string>()
//            }
//        });

//                // Configure Swagger UI for different API versions.

//                // Configure Swagger UI for Version 1.
//                options.SwaggerDoc("v1", new OpenApiInfo
//                {
//                    Version = "v1",
//                    Title = "Villa API (Version 1)",
//                    Description = "API documentation for managing Villas - Version 1",
//                    TermsOfService = new Uri("https://termsOfServices.testAPI.com"),
//                    Contact = new OpenApiContact
//                    {
//                        Name = "ContactUs",
//                        Url = new Uri("https://contactus.testAPI.com"),
//                    },
//                    License = new OpenApiLicense
//                    {
//                        Name = "API License",
//                        Url = new Uri("https://apiLicense.testAPI.com"),
//                    }
//                });

//                // Configure Swagger UI for Version 2.
//                options.SwaggerDoc("v2", new OpenApiInfo
//                {
//                    Version = "v2",
//                    Title = "Villa API (Version 2)",
//                    Description = "API documentation for managing Villas - Version 2",
//                    TermsOfService = new Uri("https://termsOfServices.testAPI.com"),
//                    Contact = new OpenApiContact
//                    {
//                        Name = "ContactUs",
//                        Url = new Uri("https://contactus.testAPI.com"),
//                    },
//                    License = new OpenApiLicense
//                    {
//                        Name = "API License",
//                        Url = new Uri("https://apiLicense.testAPI.com"),
//                    }
//                });
//            });
//        }

//        // Configuring Version settings to allow multiple versions

//        /// <summary>
//        /// Configures support for API versioning in the application.
//        /// </summary>
//        /// <param name="builder">The web application builder.</param>
//        public static void ConfigureApiVersionSupport(this WebApplicationBuilder builder)
//        {
//            // Add API versioning services to the application.
//            builder.Services.AddApiVersioning(options =>
//            {
//                // Assume the default version when the client does not specify a version.
//                options.AssumeDefaultVersionWhenUnspecified = true;

//                // Set the default API version to Major version 1.0.
//                options.DefaultApiVersion = new ApiVersion(1, 0);

//                // Include supported API versions in the response object.
//                options.ReportApiVersions = true;
//            });

//            // Add versioned API explorer services to the application.
//            builder.Services.AddVersionedApiExplorer(options =>
//            {
//                // Format API version groups as 'v{version}' (e.g., 'v1', 'v2').
//                options.GroupNameFormat = "'v'VVV";

//                // Automatically substitute the version number in URLs.
//                options.SubstituteApiVersionInUrl = true;
//            });
//        }

//        // Microsft .Net Identity setup

//        /// <summary>
//        /// Configures Identity services for the application.
//        /// </summary>
//        /// <param name="builder">The web application builder.</param>
//        public static void ConfigureIdentityServices(this WebApplicationBuilder builder)
//        {
//            // Configure Entity Framework support to enable integration with the .NET Identity system.
//            // This step is necessary to create and manage tables required for .NET Identity in the database.
//            builder.Services
//                .AddIdentity<ApplicationUser, IdentityRole>()

//                // Configure Identity to use Entity Framework as the storage mechanism.
//                // This setup enables Identity to interact with the application's database.

//                .AddEntityFrameworkStores<ApplicationDbContext>();
//        }

//        /// <summary>
//        /// Extension method to retrieve the JSON Web Token (JWT) secret from the application configuration.
//        /// </summary>
//        /// <param name="configuration">The configuration instance.</param>
//        /// <returns>The JWT secret.</returns>
//        public static string RetrieveJwtSecret(this IConfiguration configuration)
//        {
//            // Fetch the JWT secret from the application configuration using the specified key.
//            // The configuration key "ApiSettings:Secret" corresponds to the JWT secret.
//            // Note: Consider adding error handling or a default value here if necessary.
//            return configuration.GetValue<string>("ApiSettings:Secret");
//        }

//        /// <summary>
//        /// Extension method to check if a string is null or empty.
//        /// </summary>
//        /// <param name="value">The string value to check.</param>
//        /// <returns>True if the string is null or empty; otherwise, false.</returns>
//        public static bool IsNullOrEmpty(this string value)
//        {
//            // Use the built-in string.IsNullOrEmpty method for the check.
//            return string.IsNullOrEmpty(value);
//        }

//        /// <summary>
//        /// Applies pagination to an IQueryable collection by skipping a specified number of items and taking a specified number of items.
//        /// </summary>
//        /// <typeparam name="T">The type of elements in the collection.</typeparam>
//        /// <param name="query">The IQueryable collection to paginate.</param>
//        /// <param name="pageSize">The number of items to include per page.</param>
//        /// <param name="pageNumber">The page number (1-based index) of the desired page.</param>
//        /// <returns>The paginated IQueryable collection.</returns>
//        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int pageSize, int pageNumber)
//        {
//            // Calculate the number of items to skip and take based on page size and number.
//            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
//        }
//    }
//}
