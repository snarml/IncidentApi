using IncidentApiRimel.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<IncidentsDbContext>(options =>
        options.UseInMemoryDatabase("IncidentsTestsDb"));
}
else
{
    var dbPath = System.IO.Path.Combine(builder.Environment.ContentRootPath, "Incidents.db");
    builder.Services.AddDbContext<IncidentsDbContext>(options =>
        options.UseSqlite($"Data Source={dbPath}"));
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
