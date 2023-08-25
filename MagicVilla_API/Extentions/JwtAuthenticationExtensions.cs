using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MagicVilla_API.Extentions
{
    public static class JwtAuthenticationExtensions
    {
        // Configuring Authentication and JWT setup.
        /// <summary>
        /// Configures JWT authentication for the application.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        public static void ConfigureJwtAuthentication(this WebApplicationBuilder builder)
        {
            // Configure authentication services for JWT.
            var authenticationBuilder = builder.Services.AddAuthentication(options =>
            {
                // Use JWT Bearer as the default authentication scheme for both authentication and challenge.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            // Configure JWT Bearer authentication.
            authenticationBuilder.AddJwtBearer(x =>
            {
                // Require HTTPS metadata and save tokens.
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;

                // Set token validation parameters.
                x.TokenValidationParameters = new()
                {
                    // Validate the issuer signing key.
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.RetrieveJwtSecret())),

                    // Disable issuer and audience validation for flexibility.
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        /// <summary>
        /// Extension method to retrieve the JSON Web Token (JWT) secret from the application configuration.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        /// <returns>The JWT secret.</returns>
        public static string RetrieveJwtSecret(this IConfiguration configuration)
        {
            // Fetch the JWT secret from the application configuration using the specified key.
            // The configuration key "ApiSettings:Secret" corresponds to the JWT secret.
            // Note: Consider adding error handling or a default value here if necessary.
            return configuration.GetValue<string>("ApiSettings:Secret");
        }
    }
}
