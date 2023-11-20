// 1. Create webapplication builder instance

var builder = WebApplication.CreateBuilder(args);

// 2. Register middleware to builder instance

builder.Services.AddHttpLogging(opts 
    => opts.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties);

// 3. Call build to create app instance
var app = builder.Build();

// 4. Add middleware to pipeline
if (app.Environment.IsDevelopment())
{
    app.UseHttpLogging();
}

// 5. Map Endpoints
app.Map("/", () => "Hello, World!");
app.Map("/Greet", () => new Greet("Hello", "Jay!"));

// 6. Run app
app.Run();

public record Greet(string greeting, string name);