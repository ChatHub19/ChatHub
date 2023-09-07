using ChatHubProject.Application.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using ChatHubProject.Application.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// JWT Authentication ******************************************************************************
byte[] secret = Convert.FromBase64String(builder.Configuration["Secret"]);
builder.Services
    .AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secret),
            ValidateAudience = false,
            ValidateIssuer = false
        };
    });
// *************************************************************************************************

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<ChatHubContext>(opt =>
{
    opt.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        // Allow vue devserver on port 5173
        options.AddDefaultPolicy(
            builder =>
            {
                builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
                .WithOrigins("http://127.0.0.1:5173", "https://127.0.0.1:5173", "http://localhost:5173", "https://localhost:5173");
            });
    });
}

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        using (var db = scope.ServiceProvider.GetRequiredService<ChatHubContext>())
        {
            await db.CreateDatabase(isDevelopment: app.Environment.IsDevelopment());
        }
    }
    app.UseCors();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");
app.Run();