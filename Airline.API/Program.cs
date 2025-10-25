using Airline.Application;
using Airline.Application.Contracts.Flights;
using Airline.Domain;
using Airline.Domain.Entities;
using Airline.Infrastructure.EfCore;
using Airline.Infrastructure.EfCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAutoMapper(typeof(AirlineProfile));

builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IRepository<Flight, int>, FlightEfCoreRepository>();
builder.Services.AddScoped<IRepository<AircraftFamily, int>, AircraftFamilyEfCoreRepository>();
builder.Services.AddScoped<IRepository<AircraftModel, int>, AircraftModelEfCoreRepository>();
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
builder.Services.AddSwaggerGen();


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
