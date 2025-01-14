using DHBTestApplication.Application.Features.Country;
using DHBTestApplication.Application.Interface;
using DHBTestApplication.Application.Services;
using DHBTestApplication.Infrastructure;
using DHBTestApplication.Domain;
using DHBTestApplication.Domain.Services;
using DHBTestApplication.Infrastructure.Services;
using ISimilarityService = DHBTestApplication.Application.Interface.ISimilarityService;

var builder = WebApplication.CreateBuilder(args);

builder.Environment.EnvironmentName = "Development";
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddInfrastructureServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IJsonFileReader, JsonFileReader>();
builder.Services.AddScoped<ISimilarityService, SimilarityService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<GetAllCountries>();
builder.Services.AddScoped<SearchCountriesByName>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
