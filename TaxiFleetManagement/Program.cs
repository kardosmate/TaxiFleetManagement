using Microsoft.EntityFrameworkCore;
using TaxiFleetBusiness.Interfaces;
using TaxiFleetBusiness.Services;
using TaxiFleetData.Migrations;
using TaxiFleetData.Repositories;
using TaxiFleetBusiness.Mapping;
using Microsoft.OpenApi.Models;
using TaxiFleetApi.Filters;
using System.Text.Json.Serialization;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add JSON serialization options for enums
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        // Add database context with SQL Server
        builder.Services.AddDbContext<TaxiFleetDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add repositories and services
        builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
        builder.Services.AddScoped<IVehicleService, VehicleService>();
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        // Configure Swagger for API documentation
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Taxi Fleet API", Version = "v1" });
            c.UseInlineDefinitionsForEnums();
            c.SchemaFilter<EnumSchemaFilter>();
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Taxi Fleet API v1");
        });

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}