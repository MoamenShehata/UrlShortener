using Moamen.SideProjects.Infrastructure.DependencyRegistration;
using Moamen.SiderProjects.Application.DependencyRegistration;
using Moamen.SiderProjects.Application.Features.Urls.Services;
using Moamen.SiderProjects.Persistence.DependencyRegistration;
using Moamen.SiderProjects.UrlSHortener.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services
	.AddApplicationDependencies()
	.AddInfrastructureDependencies(builder.Configuration)
	.AddPersistenceDependencies(builder.Configuration);

builder.Services.AddSingleton<IHostProvider, WebHostProvider>();
builder.Services.AddHttpContextAccessor();

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

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Urls}/{action=Index}/{id?}");

app.Run();
