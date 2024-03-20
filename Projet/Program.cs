
using Microsoft.EntityFrameworkCore;
using Projet.Data;
using System;
using Microsoft.AspNetCore.Identity;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Rotativa.AspNetCore;
using NLog;
using NLog.Web;
using Microsoft.AspNetCore.Http.Features;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try { 
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddControllersWithViews();
    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

    builder.Services.AddRazorPages();
    builder.Services.AddSession(); // Add session configuration
    builder.Services.Configure<FormOptions>(p =>
        {
            p.ValueLengthLimit = int.MaxValue;
            p.MultipartBoundaryLengthLimit = int.MaxValue;
            p.MemoryBufferThreshold = int.MaxValue;
        });

    builder.Services.AddControllersWithViews();
    builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionDb")), ServiceLifetime.Scoped);

    builder.Services.AddDefaultIdentity<ApplicationUser>().AddDefaultTokenProviders().AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>();

    builder.Services.AddScoped<CommunCoursesHandler>();

    var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS loggg is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Créer des rôles si ils n'existent pas
    var roleNames = new string[] { "Admin", "Coordonnateur", "Enseignant" ,"Directeur","Chef" , "Secritaire" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapRazorPages();
app.UseRouting();
app.UseAuthentication();;
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
app.UseAuthorization();
app.UseSession();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "Responsable",
      pattern: "{ area: exists}/{ controller}/{ action}/{ id ?}"

    );
});
app.MapControllerRoute(
    name: "Coordenateur",
    pattern: "{area:exists}/{controller}/{action}/{id?}");

app.MapControllerRoute(
    name: "Coordenateur",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Coordenateur",
    pattern: "{area:exists}/{controller=Emploi}/{action}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
RotativaConfiguration.Setup(app.Environment.WebRootPath, @"asset/lib/rotativa-aspnetcore");
app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}