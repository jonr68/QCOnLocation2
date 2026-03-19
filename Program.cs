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

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LocationContext>(options =>
    options.UseSqlite("Data Source=location.db"));

// Development-only: allow the front-end test page (served from http://localhost:5267)
// to call this API (including PUT) and allow cookies to be sent for authentication.
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

// Configure authentication: cookies + optional Google OAuth
// ClientId and ClientSecret should be set in configuration (e.g. appsettings.json or environment variables):
// "Authentication:Google:ClientId" and "Authentication:Google:ClientSecret"
var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
var googleConfigured = !string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret);

builder.Services.AddAuthentication(options =>
{
    // Use cookies as the main sign-in scheme
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    // When an unauthenticated request needs to challenge, use Google if configured
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
        // options.CallbackPath = "/signin-google";
    });
}

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // Enable CORS for local testing from http://localhost:5267
    app.UseCors("LocalDev");

    // Serve static files from the content root so files inside the project can be requested
    // while debugging. This is intentionally development-only.
    var provider = new PhysicalFileProvider(app.Environment.ContentRootPath);
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = provider,
        RequestPath = ""
    });

    // Also serve a test page located in the parent folder (workspace root) if present.
    // Many users keep a top-level `test-put.html` outside the project folder; this
    // makes that file available at /test-put.html when running locally.
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

// Standard authentication/authorization middleware. Controllers will be annotated
// with [Authorize] / [AllowAnonymous] so only intended endpoints require login.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllers();

app.Run();
