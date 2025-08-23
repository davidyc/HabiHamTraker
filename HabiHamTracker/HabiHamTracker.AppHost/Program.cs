var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.HabiHamTracker>("habihamtracker");

builder.Build().Run();
