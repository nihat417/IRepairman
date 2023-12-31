using IRepairman.Application.Interfaces;
using IRepairman.Application.Models;
using IRepairman.Domain.Entities;
using IRepairman.Helpers;
using IRepairman.Persistence.Datas;
using IRepairman.Persistence.Repository;
using IRepairman.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("default")));
builder.Services.AddSignalR();
builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(op => op.SignIn.RequireConfirmedEmail = true);
builder.Services.Configure<DataProtectionTokenProviderOptions>(op => op.TokenLifespan = TimeSpan.FromHours(10));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();

builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISpecializationRepository, SpecializationRepository>();
builder.Services.AddSingleton(emailConfig);
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
app.MapHub<ChatHub>("/chatHub");

app.UseEndpoints(endpoints =>
{
	endpoints.MapAreaControllerRoute(
		name: "ForeAdminarea",
		areaName: "Admin",
		pattern: "foradmin/{controller=Admin}/{action=AdminPage}");

	endpoints.MapAreaControllerRoute(
		name: "ForeMasterarea",
		areaName: "Master",
		pattern: "formaster/{controller=Master}/{action=MasterPage}");

	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Main}/{action=Index}/{id?}");

	endpoints.MapControllerRoute(
		name: "chat",
		pattern: "chat/{userId}",
		defaults: new { controller = "Chat", action = "Index" });

	endpoints.MapControllerRoute(
		name: "messages",
		pattern: "Messages/{action=Index}/{id?}",
		defaults: new { controller = "Messages" });
});
await SeedData.InitializeAsync(app.Services);

app.Run();
