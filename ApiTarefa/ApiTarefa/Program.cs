using Microsoft.EntityFrameworkCore;

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

//Endpoints
app.MapGet("/",()=> "Olá Mundo");
app.MapGet("Frases", async () => await new HttpClient().GetStreamAsync("https://ron-swanson-quotes.herokuapp.com/v2/quotes"));




app.Run();


class Tarefas
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public bool IsConcluida { get; set; }
}

class AppDbContext : DbContext
{

}