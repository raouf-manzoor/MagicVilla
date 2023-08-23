using MagicVilla_API;
using MagicVilla_API.Data;
using MagicVilla_API.Repository;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Serilog Logger Configuration

//Log.Logger=new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .WriteTo.File("log/villaLogs.txt",rollingInterval:RollingInterval.Day)
//    .CreateLogger();

//builder.Host.UseSerilog();

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

// Configuring response caching.

builder.Services.AddResponseCaching();

builder.Services.AddAutoMapper(typeof(MappingConfig));

// Replaced them with extention method
builder.RegisterDependencies();

// Add Api Versioning

//builder.Services.AddApiVersioning(options =>
//{
//    // In case if no version is supplied by the client. Setting up true to avoid exception
//    options.AssumeDefaultVersionWhenUnspecified = true;

//    // Setting up Major and minor versions
//    options.DefaultApiVersion = new ApiVersion(1, 0);

//    // This will add supported Api versions in the response object
//    options.ReportApiVersions = true;
//});

//builder.Services.AddVersionedApiExplorer(options =>
//{
//    options.GroupNameFormat = "'v'VVV";

//    // Instead of passing version manually we will set this flag to true to automatically substitue the version number

//    options.SubstituteApiVersionInUrl = true;
//});

builder.AddApiVersionSupport();

// Add new extention method for configuration of JWT 
builder.JWTConfiguration();

builder.Services.AddControllers(
    //option=>option.ReturnHttpNotAcceptable=true

    // Configuring caching profile.
    option => {

        option.CacheProfiles.Add("Default30", new CacheProfile()
        {
            Duration = 30
        }); ;
    }

    )
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add new extention method for configuration of Swagger
builder.SwaggerConfiguration();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        // Configuring swagger UI to make it more dynamic. The default document which will pop up

        // Version 1 configuration

        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Magic_VillaV1");

        // Version 2 configuration
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Magic_VillaV2");

    });
}

app.UseHttpsRedirection();

// UseAuthentication must come before UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
