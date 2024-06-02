using EnterpriseMvcApp.Data;
using EnterpriseMvcApp.Middleware;
using EnterpriseMvcApp.Repositories;
using EnterpriseMvcApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure MongoDB settings
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));

// Register MongoDbContext
builder.Services.AddSingleton<MongoDbContext>();

// Register PostgreSQL context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IContentRepository, ContentRepository>();

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IContentService, ContentService>();

// Register DataSeeder
builder.Services.AddTransient<DataSeeder>();

// Configure authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/User/Login";
            options.LogoutPath = "/User/Logout";
        });

// Configure logging
//Log.Logger = new LoggerConfiguration()
//	.ReadFrom.Configuration(builder.Configuration)
//	.CreateLogger();

Log.Logger = new LoggerConfiguration()
		   .MinimumLevel.Debug()
		   .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
		   .Enrich.FromLogContext()
		   .WriteTo.Console()
		   .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
		   .CreateLogger();
//builder.Services.AddSerilog();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "EnterpriseMvcApp API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "EnterpriseMvcApp API v1");
});

app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();
