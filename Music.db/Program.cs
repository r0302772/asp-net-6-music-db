using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Music.db.Data;
using Music.db.Data.Repository;
using Music.db.Data.UnitOfWork;
using Music.db.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
#region Database Connection
var connectionString = builder.Configuration.GetConnectionString("LocalDbConnection");
builder.Services.AddDbContext<MusicdbContext>(options => options.UseSqlServer(connectionString));
#endregion
#region Repository & UnitOfWork
builder.Services.AddScoped<IGenericRepository<Song>,GenericRepository<Song>>();
builder.Services.AddScoped<IGenericRepository<Genre>,GenericRepository<Genre>>();

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
#endregion
#region Identity
builder.Services.AddDefaultIdentity<IdentityUser>()
	.AddEntityFrameworkStores<MusicdbContext>();
#endregion
var app = builder.Build();

#region Seed Data
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	try
	{
		var context = services.GetRequiredService<MusicdbContext>();
		DbInitializer.Initialize(context);
	}
	catch (Exception ex)
	{
		var logger = services.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "An error occurred while seeding the database.");
	}
}
#endregion

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

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
