
using Microsoft.EntityFrameworkCore;
using Nexus_Manegement;
using Nexus_Manegement.Models;
using Twilio.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddDbContextFactory<NexusDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectSQL:value2"))
);
Console.WriteLine("In ra chuoi ket noi: " + builder.Configuration.GetValue<string>("ConnectSQL:value"));
Console.WriteLine(builder.Configuration.GetValue<string>("ConnectSQL:value2"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "{controller=EmployeeT7}/{action=Index}/{id?}"
    );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapAreaControllerRoute(
    name: "Management",
    areaName: "Management",
    pattern: "{controller=Management}/{action=Index}/{id?}"
    );

app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "{controller=Login}/{action=Index}/{id?}"
    );
app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "{controller=PositionV3}/{action=Index}/{id?}"
    );
app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Accountant",
    pattern: "{controller=Accountant}/{action=Index}/{id?}"
    );
app.MapAreaControllerRoute(
    name: "Technical",
    areaName: "Technical",
    pattern: "{controller=Technical}/{action=Index}/{id?}"
    );
app.Run();
