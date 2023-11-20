// Add a list of friends and be able to query all, by name and by age

using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Is this middleware? 
var _friends = new List<Friend>
{
    new("Jay", 23),
    new("Chris", 22),
    new("Adric", 23)
};

app.Map("/friends", () => _friends);

app.Map("/friends/{name}", (string name) => 
    _friends.Where(p => p.Name.StartsWith(name)));

app.Map("/friends/age/{age}", (int age) => 
    _friends.Where(p => p.Age.Equals(age)));

app.Run();

public record Friend(string Name, int Age);