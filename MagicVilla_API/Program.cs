using MagicVilla_API;
using MagicVilla_API.Data;
using MagicVilla_API.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// Create the web application builder
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Set up Serilog Logger Configuration
// Define and configure the Serilog logger for application logging.
// Minimum log level is set to Debug.
// Log entries are written to "log/villaLogs.txt" file, with daily rolling interval.

//Log.Logger=new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .WriteTo.File("log/villaLogs.txt",rollingInterval:RollingInterval.Day)
//    .CreateLogger();

//builder.Host.UseSerilog();

// Set up DbContext for Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    // Configure the DbContext to use a SQL Server database.
    // Connection string is retrieved from configuration.
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

builder.ConfigureIdentityServices();

// Configure response caching
builder.Services.AddResponseCaching();
// Enable caching of API responses to improve performance.

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));
// Set up AutoMapper to handle object mapping based on MappingConfig class.


// Register application dependencies
// Use a custom extension method to register various dependencies in the container.
// This method registers services based on interfaces and implementations.
builder.RegisterDependencies();

// Configure API version support
builder.ConfigureApiVersionSupport();
// Set up API versioning in the application
// This involves supported API versions and their behavior.

// Configure JWT authentication
builder.ConfigureJwtAuthentication();
// Set up JSON Web Token (JWT) authentication for API security.
// This method configures authentication schemes, options, and related settings.

// Add controllers to the service collection
builder.Services.AddControllers(
    //option=>option.ReturnHttpNotAcceptable=true

    // Configure caching profile.
    option => {

        // Configure a caching profile named "Default30".
        // API responses with this cache profile will be cached for 30 seconds.
        option.CacheProfiles.Add("Default30", new CacheProfile()
        {
            Duration = 30
        });
    }

    )
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();
// Configure and add controllers to the application.
// Support both JSON and XML serialization formats.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Configure API endpoint documentation using Swagger
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.ConfigureSwagger();
// Set up Swagger/OpenAPI documentation for the API.
// This involves specifying endpoints and version-specific information.

// Build the application
var app = builder.Build();
// Complete building the web application.


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI for development environment
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

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();
// Automatically redirect HTTP requests to their HTTPS counterparts.

// UseAuthentication must come before UseAuthorization
app.UseAuthentication();
// Enable authentication middleware.

app.UseAuthorization();
// Enable authorization middleware.

// Map controllers
app.MapControllers();
// Map routes for the controllers.

app.Run();
