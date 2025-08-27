var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.HabiHamTracker>("habihamtracker");

var ui = builder.AddNpmApp("habihamtracker-ui", "../HabiHamTracker-ui", "dev")
    .WithHttpEndpoint(env: "PORT", targetPort: 5173)
    .WithReference(api);

builder.Build().Run();
