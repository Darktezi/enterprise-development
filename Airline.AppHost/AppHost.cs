var builder = DistributedApplication.CreateBuilder(args);
var dbPassword = builder.AddParameter("DbPassword");

var sql = builder.AddSqlServer("airline-sql", password: dbPassword)
                 .WithEnvironment("TrustServerCertificate", "true")
                 .WithEnvironment("Encrypt", "false")
                 .WithEnvironment("ACCEPT_EULA", "Y") 
                 .AddDatabase("AirlineDb");

builder.AddProject<Projects.Airline_API>("airline-api")
       .WithReference(sql, "Database")
       .WaitFor(sql);

builder.Build().Run();