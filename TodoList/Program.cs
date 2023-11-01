var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var todoItemController = new TodoItemController();

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
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/items", () => {
    return todoItemController.getAllTodoItems();    
})
.WithName("GetAllTodoItems")
.WithOpenApi();

app.MapGet("/item/", (Guid id) => {
    return todoItemController.getTodoItem(id);
});

app.MapPatch("/item/", (Guid guid, string title, string details, string startTime, string endTime) => {
    return todoItemController.updateTodoItem(guid, title, details, DateTime.ParseExact(startTime, "dd-MM-yyyy HH:mm:ss", null), DateTime.ParseExact(endTime, "dd-MM-yyyy HH:mm:ss", null));
});

app.MapPost("/item/", (string title, string details, string startTime, string endTime) => {
    return todoItemController.AddTodoItem(title, details, DateTime.ParseExact(startTime, "dd-MM-yyyy HH:mm:ss", null), DateTime.ParseExact(endTime, "dd-MM-yyyy HH:mm:ss", null));    
});

app.MapDelete("/item/", (Guid guid) => {
    return todoItemController.deleteTodoItem(guid);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
