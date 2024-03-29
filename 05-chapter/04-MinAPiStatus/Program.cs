// What I learned:
// You can manually return specific responses
// When returning you can create 'new { x = "" }' to return json content in body
// The difference between Result and Typed Results
// Simple to handle different CRUD options 
// Accessing http response directly

using System.Collections.Concurrent;
using System.Net.Mime;

// Create builder
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

// Build
WebApplication app = builder.Build();

// Mapping endpoings
var _project = new ConcurrentDictionary<string, Project>();

app.MapGet("/project", () => _project);

app.MapGet("/project/{id}", (string id) => 
    _project.TryGetValue(id, out var project)
        ? TypedResults.Ok(project)
        : Results.NoContent()
);

app.MapPost("/project/{id}", (string id, Project project) => 
    _project.TryAdd(id, project)
        ? TypedResults.Created($"/project/{id}", project)
        : Results.BadRequest(new { id = "A project with that id already exists" })
);

app.MapPut("/project/{id}", (string id, Project project) => 
{
    _project[id] = project;
    return Results.NoContent();
});

app.MapDelete("/project/{id}", (string id) => 
{
    _project.TryRemove(id, out _);
    return Results.NoContent();
});

app.MapGet("/pb", (HttpResponse response) => 
{
    response.StatusCode = 418;
    response.ContentType = MediaTypeNames.Text.Plain;
    return response.WriteAsync("Im peanut butter!");
});

app.Run();

record Project(string Name, int Difficulty);
