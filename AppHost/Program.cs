
var builder = DistributedApplication.CreateBuilder(args);

var webApi = builder.AddProject<Projects.WebAPI>("WebApi");
builder.AddProject<Projects.ClinicaNeo>("ClinicaNeo")
    .WaitFor(webApi);

builder.Build().Run();
