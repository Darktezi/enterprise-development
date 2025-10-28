using Airline.Application;
using Airline.Application.Contracts.Flight;
using Airline.Application.Contracts.Passenger;
using Airline.Application.Services;
using Airline.Domain;
using Airline.Domain.Entities;
using Airline.Infrastructure.EfCore;
using Airline.Infrastructure.EfCore.Repositories;
using Airline.ServiceDefaults;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new AirlineProfile());
});

builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IPassengerService, PassengerService>();
builder.Services.AddScoped<IRepository<Flight, int>, FlightEfCoreRepository>();
builder.Services.AddScoped<IRepository<AirlineFamily, int>, AircraftFamilyEfCoreRepository>();
builder.Services.AddScoped<IRepository<AirlineModel, int>, AircraftModelEfCoreRepository>();
builder.Services.AddScoped<IRepository<Passenger, int>, PassengerEfCoreRepository>();
builder.Services.AddScoped<IRepository<Ticket, int>, TicketEfCoreRepository>();

builder.AddSqlServerDbContext<AirlineDbContext>("Database",
    configureDbContextOptions: db =>
        db.UseLazyLoadingProxies());

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var basePath = AppContext.BaseDirectory;

    options.IncludeXmlComments(Path.Combine(basePath, "Airline.API.xml"));
    options.IncludeXmlComments(Path.Combine(basePath, "Airline.Domain.xml"));
    options.IncludeXmlComments(Path.Combine(basePath, "Airline.Application.xml"));
});

var app = builder.Build();


app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AirlineDbContext>();
    db.Database.Migrate();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
