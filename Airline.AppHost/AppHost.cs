var builder = DistributedApplication.CreateBuilder(args);

var dbPassword = builder.AddParameter("DbPassword", secret: true);

var sql = builder.AddSqlServer("airline-sql", password: dbPassword)
                 .AddDatabase("AirlineDb");

// gRPC генератор
var generator = builder.AddProject<Projects.Airline_Grpc_Client>("airline-grpc-client")
       .WithHttpsEndpoint(port: 7100, name: "grpc");

builder.AddProject<Projects.Airline_Api>("airline-api")
       .WithReference(sql, "Database")
       .WithEnvironment("GeneratorGrpcUrl", generator.GetEndpoint("grpc"))
       .WaitFor(sql)
       .WaitFor(generator);

builder.Build().Run();
