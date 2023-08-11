using MagicVilla_API;
using MagicVilla_Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(MappingConfig));

//builder.Services.AddHttpClient<IVillaService, VillaService>();
//builder.Services.AddScoped<IVillaService, VillaService>();

//builder.Services.AddHttpClient<IVillaNumberService, VillaNumberService>();
//builder.Services.AddScoped<IVillaNumberService, VillaNumberService>();

// Replaced by extention method for better understanding of service registration

builder.RegisterDependencies();
builder.ConfigureAuthenticationSettings();
builder.ConfigureSessionSettings();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
