using Carter;
using SampleCrud;
using SampleCrud.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSingleton(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString =
        configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "Connection string not provided in appsettings.json"
        );

    Console.WriteLine("CONNECTION STRING: " + connectionString);

    return new DbConnProvider(connectionString);
});

builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddCarter();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapCarter();
app.Run();
