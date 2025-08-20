using DistanceService.Application.Interfaces;
using DistanceService.Application.Services;
using DistanceService.Infrastructure.Repositories;
using DistanceService.Infrastructure.Services;
using DistanceService.Infrastructure.Auth;
using DistanceService.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<AirportApiOptions>(builder.Configuration.GetSection("AirportApi"));
builder.Services.Configure<DistanceOptions>(builder.Configuration.GetSection("Distance"));
builder.Services.Configure<CacheOptions>(builder.Configuration.GetSection("Cache"));
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Auth"));

builder.Services.AddHttpClient<IAirportRepository, HttpAirportRepository>();

builder.Services.AddSingleton(typeof(ICacheService<>), typeof(BoundedMemoryCacheService<>));
builder.Services.AddScoped<IAirportService, AirportService>();

builder.Services.AddAuthentication("BearerCode")
    .AddScheme<AuthenticationSchemeOptions, BearerCodeAuthHandler>("BearerCode", options => { });
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<DistanceService.Middleware.ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/healthz", () => Results.Ok("OK")).AllowAnonymous();

app.MapControllers();

app.Run();