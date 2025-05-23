using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(); 
builder.Services.AddSingleton<ConcurrentBag<string>>();
builder.WebHost.UseUrls("http://0.0.0.0:8080");

var app = builder.Build();


app.MapGet("/notes", static ([FromServices] ConcurrentBag<string> notes) =>
{
    return Results.Ok(notes); 
});

app.MapPost("/notes", static async (HttpContext context, [FromServices] ConcurrentBag<string> notes) =>
{
    var dto = await context.Request.ReadFromJsonAsync<MessageDTO>();
    if (dto == null)
    {
        return Results.InternalServerError("Du sendte ikke valid besked!");
    }
    notes.Add(dto.msg);
    return Results.NoContent();
});

app.Run();

public record MessageDTO(string msg); 

