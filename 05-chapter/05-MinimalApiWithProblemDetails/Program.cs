using System.Collections.Concurrent;
using System.Net.Mime;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
WebApplication app = builder.Build();

var _project = new ConcurrentDictionary<string, Project>();

app.MapGet("/project", () => _project);

app.MapGet("/project/{id}", (string id) => 
    _project.TryGetValue(id, out var project)
        ? TypedResults.Ok(project)
        : Results.Problem(statusCode: 404)
);

app.MapPost("/project/{id}", (string id, Project project) => 
    _project.TryAdd(id, project)
        ? TypedResults.Created($"/project/{id}", project)
        : Results.ValidationProblem(new Dictionary <string, string[]>
            {
                {"id", new[] {"A project with this id already exists"} }
        }));

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
