using DistanceService.Adapters.Authentication;
using DistanceService.Adapters.Middleware;
using DistanceService.Adapters.Repositories;
using DistanceService.Adapters.Services;
using DistanceService.Domain;
using DistanceService.Domain.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<DistanceOptions>(builder.Configuration.GetSection("Distance"));
builder.Services.Configure<AirportApiOptions>(builder.Configuration.GetSection("AirportApi"));
builder.Services.Configure<CacheOptions>(builder.Configuration.GetSection("Cache"));
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Auth"));

builder.Services.AddScoped<IAirportService, AirportService>();
builder.Services.AddHttpClient<IAirportRepository, HttpAirportRepository>();
builder.Services.AddSingleton(typeof(ICacheService<>), typeof(BoundedMemoryCacheService<>));

builder.Services.AddAuthentication("Bearer")
    .AddScheme<AuthenticationSchemeOptions, BearerTokenAuthenticationHandler>("Bearer", null);
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Enter the bearer token.",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/healthz", () => Results.Ok("OK"))
   .ExcludeFromDescription()
   .AllowAnonymous();

app.MapControllers();
app.MapSwagger();
app.UseSwaggerUI();

app.Run();
