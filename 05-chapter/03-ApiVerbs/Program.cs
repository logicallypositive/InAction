// Handlers can be 
// 1. lambda expressions 
// 2. variables storing lambda expressions
// 3. static methods
// 4. instance methods
// 5. local functions

// Create Builder   
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Register services / configuration
builder.Services.AddEndpointsApiExplorer();

// Initialize app
WebApplication app = builder.Build();

// Map endpoints (get, get, post, put, delete)

app.MapGet("/project", () => Project.All);

Delegate getProject =  (string id) => Project.All[id];
app.MapGet("/project/{id}", getProject);

app.MapPost("/project/{id}", Handlers.AddProject);

Handlers handlers = new();
app.MapPut("project/{id}", () => handlers.ReplaceProject);

app.MapDelete("project/{id}", () => DeleteProject);

app.Run();

void DeleteProject (string id)
{
    Project.All.Remove(id);
}

public record Project(string Name, int Difficulty)
{
    public static readonly Dictionary<string, Project> All = new();
};

public class Handlers
{
    public void ReplaceProject (string id, Project project)
    {
        Project.All[id] = project;
    }

    public static void AddProject (string id, Project project)
    {
        Project.All.Add(id, project);
    }
}

