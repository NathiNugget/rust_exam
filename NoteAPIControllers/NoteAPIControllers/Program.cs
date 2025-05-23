using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ConcurrentBag<string>>();
builder.Services.AddControllers();
builder.WebHost.UseUrls("http://0.0.0.0:8080"); 
var app = builder.Build();

app.MapControllers();

app.Run();

