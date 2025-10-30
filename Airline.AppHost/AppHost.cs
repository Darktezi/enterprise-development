var builder = DistributedApplication.CreateBuilder(args);
var dbPassword = builder.AddParameter("DbPassword");

var sql = builder.AddSqlServer("airline-sql", password: dbPassword)
                 .AddDatabase("AirlineDb");

builder.AddProject<Projects.Airline_Api>("airline-api")
       .WithReference(sql, "Database")
       .WaitFor(sql);

builder.Build().Run();