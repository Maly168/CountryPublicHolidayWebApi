using CountryPublicHolidayWebApi.Filters;
using CountryPublicHolidayWebApi.Service;
using DataAccess.DbContexts;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>();
builder.Services.AddHttpClient<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<ICountryService, CountryService>();
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
        document.Info.Title = "Country Api";
        document.Info.Description = "List all the calendar in some countries";
        document.Info.TermsOfService = "None";
        document.Info.Contact = new NSwag.OpenApiContact
        {
            Name = "Maly",
            Email = string.Empty,
            Url = "https://twitter.com/spboyer"
        };
        document.Info.License = new NSwag.OpenApiLicense
        {
            Name = "Use under LICX",
            Url = "https://example.com/license"
        };
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
