using Scalar.AspNetCore;
using SpiderMem.API.Extensions;
using SpiderMem.Application;
using SpiderMem.Application.Common;
using SpiderMem.API.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
var key = builder.Configuration["Jwt:Key"];

var keyBytes = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // dev only
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
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserAccessor, UserAccessor>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<SpiderMem.Application.Queries.GetMemes.GetMemesQuery>());
builder.Services.AddControllers();
builder.Services.AddApiServices(builder.Configuration);



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseCors("AllowVite");
// Configure the HTTP request pipeline.
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

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
