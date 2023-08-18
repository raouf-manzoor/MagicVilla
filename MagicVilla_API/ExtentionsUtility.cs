﻿using MagicVilla_API.Repository.IRepository;
using MagicVilla_API.Repository;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.OpenApi.Models;

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

        public static void JWTConfiguration(
            this WebApplicationBuilder builder)
        {
            var authenticationBuilder = builder.Services.AddAuthentication(options =>
             {
                 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
             });

            authenticationBuilder.AddJwtBearer(x =>
          {
              x.RequireHttpsMetadata = false;
              x.SaveToken = true;
              x.TokenValidationParameters = new()
              {
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetJWTSecret())),
                  ValidateIssuer = false,
                  ValidateAudience = false,

              };
          });
        }

        public static void SwaggerConfiguration(
            this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                        "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                        "Example: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
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

                // Adding this configuration to customize the swagger document UI

                // Version 1 UI config
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Villa API",
                    Description = "Management of Villas",
                    TermsOfService = new Uri("https://termsOfServices.testAPI.com"),

                    Contact = new OpenApiContact
                    {
                        Name = "ContactUs",
                        Url = new Uri("https://contactus.testAPI.com"),
                    },

                    License = new OpenApiLicense
                    {
                        Name = "APi License",
                        Url = new Uri("https://apiLicense.testAPI.com"),
                    }



                });

                // Version 2 UI config
                options.SwaggerDoc("v2", new OpenApiInfo()
                {
                    Version = "v2",
                    Title = "Villa API 2",
                    Description = "Management of Villas",
                    TermsOfService = new Uri("https://termsOfServices.testAPI.com"),

                    Contact = new OpenApiContact
                    {
                        Name = "ContactUs",
                        Url = new Uri("https://contactus.testAPI.com"),
                    },

                    License = new OpenApiLicense
                    {
                        Name = "APi License",
                        Url = new Uri("https://apiLicense.testAPI.com"),
                    }



                });

            });
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
