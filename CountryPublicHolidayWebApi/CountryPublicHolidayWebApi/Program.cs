using CountryPublicHolidayWebApi.Filters;
using CountryPublicHolidayWebApi.Services;
using DataAccess.DbContexts;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>();
builder.Services.AddHttpClient<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<ICountryHolidayService, CountryHolidayService>();
builder.Services.TryAddSingleton<RequestFilter>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerDocument(config =>
{
    config.PostProcess = document =>
    {
        document.Info.Version = "v1";
        document.Info.Title = "CountryPublicHolidayApi";
        document.Info.Description = "List public holiday api and supported countries";
        document.Info.TermsOfService = "None";
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseOpenApi();
app.UseSwaggerUi3();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
