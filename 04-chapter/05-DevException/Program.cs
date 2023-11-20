var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDeveloperExceptionPage(); // used by default
app.MapGet("/", () => BadService.GetValues());

app.Run();

class BadService
{
    public static string? GetValues()
    {
        throw new Exception("Ooops");
    }
}