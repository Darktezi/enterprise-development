using Airline.Grpc.Client;
using Airline.ServiceDefaults;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Регистрируем gRPC сервер
builder.Services.AddGrpc();

var app = builder.Build();

app.MapDefaultEndpoints();

// Маппим gRPC сервис
app.MapGrpcService<TicketGeneratorGrpcService>();

app.MapGet("/", () => "Ticket Generator gRPC Server");

app.Run();
