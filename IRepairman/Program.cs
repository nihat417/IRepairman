using IRepairman.Domain.Entities;
using IRepairman.Persistence.Datas;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("default")));
builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(op => op.SignIn.RequireConfirmedEmail = true);
builder.Services.Configure<DataProtectionTokenProviderOptions>(op => op.TokenLifespan = TimeSpan.FromHours(10));
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

app.UseAuthorization();

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
});


app.Run();