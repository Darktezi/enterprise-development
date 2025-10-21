var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Airline_API>("airline-api");

builder.Build().Run();
