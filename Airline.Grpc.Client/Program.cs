using Airline.Api.Grpc;
using Airline.Grpc.Client;
using Airline.ServiceDefaults;
using Grpc.Net.Client;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton(serviceProvider =>
{
    var apiGrpcUrl = builder.Configuration["ApiGrpcUrl"] ??
        throw new InvalidOperationException("ApiGrpcUrl не найден в конфигурации");

    var channel = GrpcChannel.ForAddress(apiGrpcUrl);
    return new TicketReceiver.TicketReceiverClient(channel);
});

var host = builder.Build();

host.Services.GetRequiredService<TicketReceiver.TicketReceiverClient>();

host.Run();
