var builder = DistributedApplication.CreateBuilder(args);

var ticketReceiverConfig = builder.Configuration.GetSection("TicketReceiver");
var batchSize = ticketReceiverConfig["BatchSize"] ?? "50";
var payloadLimit = ticketReceiverConfig["PayloadLimitBytes"] ?? "5242880";
var maxReceiveSize = ticketReceiverConfig["MaxReceiveMessageSizeBytes"] ?? "10485760";
var maxSendSize = ticketReceiverConfig["MaxSendMessageSizeBytes"] ?? "10485760";
var workerDelay = builder.Configuration["Worker:DelaySeconds"] ?? "1";

var dbPassword = builder.AddParameter("DbPassword");

var sql = builder.AddSqlServer("airline-sql", password: dbPassword)
                 .AddDatabase("AirlineDb");

var api = builder.AddProject<Projects.Airline_Api>("airline-api")
       .WithReference(sql, "Database")
       .WithEnvironment("TicketReceiver__BatchSize", batchSize)
       .WithEnvironment("TicketReceiver__PayloadLimitBytes", payloadLimit)
       .WithEnvironment("TicketReceiver__MaxReceiveMessageSizeBytes", maxReceiveSize)
       .WithEnvironment("TicketReceiver__MaxSendMessageSizeBytes", maxSendSize)
       .WaitFor(sql);

builder.AddProject<Projects.Airline_Grpc_Client>("airline-grpc-client")
       .WithEnvironment("WorkerDelaySeconds", workerDelay)
       .WithEnvironment("ApiGrpcUrl", api.GetEndpoint("https"))
       .WaitFor(api);

builder.Build().Run();
