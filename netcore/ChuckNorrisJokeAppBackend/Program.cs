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

app.MapGet("/joke", async (context) =>
{
    using var httpClient = new HttpClient();
    var chuckNorrisUrl = "https://api.chucknorris.io/jokes/random";
    var response = await httpClient.GetAsync(chuckNorrisUrl);

    if (response.IsSuccessStatusCode)
    {
        var contentString = await response.Content.ReadAsStringAsync();
        var jokeData = System.Text.Json.JsonSerializer.Deserialize<dynamic>(contentString);
        var newJokeObject = new { joke = jokeData.value };
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(newJokeObject));
    }
    else
    {
        context.Response.StatusCode = response.StatusCode;
        await context.Response.WriteAsync("Failed to retrieve joke from Chuck Norris API.");
    }
});
.WithName("GetChuckNorrisJoke")
.WithOpenApi();

app.Run();
