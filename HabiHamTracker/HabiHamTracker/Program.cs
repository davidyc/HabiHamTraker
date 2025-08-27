using HabiHamTracker.Services.Interfaces;
using HabiHamTracker.Services;
using Scalar.AspNetCore;
using HabiHamTracker.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// Add services to the container.
builder.Services.AddScoped<IWeightService, WeightService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((doc, ctx, ct) =>
    {
        doc.Servers.Clear();
        return Task.CompletedTask;
    });
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate(); 
    }
    catch (Exception ex)
    {
       
        app.Logger.LogError(ex, "�� ������� ��������� �������� ��");
        throw;
    }
}


app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/");
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
