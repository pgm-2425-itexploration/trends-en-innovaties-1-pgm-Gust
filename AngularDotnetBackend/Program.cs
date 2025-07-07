// Simple .NET backend for a task list (no database)
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// CORS for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Activeer CORS vóór de endpoints
app.UseCors("AllowAngularDev");

// In-memory list of tasks
var tasks = new List<TaskItem>();

// Get all tasks
app.MapGet("/api/tasks", () => tasks);

// Add a new task
app.MapPost("/api/tasks", (TaskItem task) =>
{
    task.Id = tasks.Count > 0 ? tasks[^1].Id + 1 : 1;
    tasks.Add(task);
    return Results.Created($"/api/tasks/{task.Id}", task);
});

// Update a task
app.MapPut("/api/tasks/{id}", (int id, TaskItem updatedTask) =>
{
    var task = tasks.Find(t => t.Id == id);
    if (task is null) return Results.NotFound();
    task.Title = updatedTask.Title;
    task.IsCompleted = updatedTask.IsCompleted;
    return Results.NoContent();
});

// Delete a task
app.MapDelete("/api/tasks/{id}", (int id) =>
{
    var task = tasks.Find(t => t.Id == id);
    if (task is null) return Results.NotFound();
    tasks.Remove(task);
    return Results.NoContent();
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// Simple task model
record TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}
