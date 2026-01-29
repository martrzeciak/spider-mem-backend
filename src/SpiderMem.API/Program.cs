using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using SpiderMem.API.Extensions;
using SpiderMem.API.Helpers;
using SpiderMem.API.Services;
using SpiderMem.Application.Behaviors;
using SpiderMem.Application.Common;
using SpiderMem.Application.Interfaces;
using SpiderMem.Application.Queries.GetMemes;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVite", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Authentication & Authorization
var keyBytes = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
    };
});

// Application Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserAccessor, UserAccessor>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssemblyContaining<GetMemesQuery>();
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddApiServices(builder.Configuration);

// Configuration
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings")
);

// Controllers & OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();


var app = builder.Build();

// CORS Middleware
app.UseCors("AllowVite");

// Development Environment Configuration
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithDefaultHttpClient(
            ScalarTarget.CSharp,
            ScalarClient.HttpClient);
    });
}

// Authentication & Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapControllers();

app.Run();
