using System.Collections.Concurrent;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails(); 
builder.Services.AddEndpointsApiExplorer();
WebApplication app = builder.Build();

{
    app.UseExceptionHandler();
}
app.UseStatusCodePages();

var _project = new ConcurrentDictionary<string, Project>();

app.MapGet("/", void () => throw new Exception("Demonstrating automatic ProblemDetails"));

app.MapGet("/project", () => _project);

app.MapGet("/project/{id}", (string id) =>
    _project.TryGetValue(id, out var project)
        ? TypedResults.Ok(project)
        : Results.NotFound()); // standard error converted to ProblemDetails

app.MapPost("/project/{id}", (string id, Project project) =>
    _project.TryAdd(id, project)
        ? TypedResults.Created($"/project/{id}", project)
        : Results.ValidationProblem(new Dictionary<string, string[]>
          {
              { "id", new[] { "A project with this id already exists" } }
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

app.Run();
record Project(string Name, int Stock);