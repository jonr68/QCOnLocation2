using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using QcOnLocation.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LocationContext>(options =>
    options.UseSqlite("Data Source=location.db"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalDev", policy =>
    {
        policy.WithOrigins("http://localhost:5267")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
var googleConfigured = !string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        if (googleConfigured)
        {
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        }
    })
    .AddCookie();

if (googleConfigured)
{
    builder.Services.AddAuthentication().AddGoogle(options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
    });
}

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("LocalDev");

    var provider = new PhysicalFileProvider(app.Environment.ContentRootPath);
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = provider,
        RequestPath = ""
    });

    try
    {
        var parentPath = Path.GetFullPath(Path.Combine(app.Environment.ContentRootPath, ".."));
        var parentProvider = new PhysicalFileProvider(parentPath);
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = parentProvider,
            RequestPath = ""
        });
    }
    catch
    {
        // If for any reason we can't expose parent folder, ignore — it's only a convenience.
    }
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllers();

app.Run();