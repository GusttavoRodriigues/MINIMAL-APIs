using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registrando contexto como serviço
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("TarefasDB"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



//Endpoints
app.MapGet("/",()=> "Olá Mundo");
app.MapGet("Frases", async () => await new HttpClient().GetStreamAsync("https://ron-swanson-quotes.herokuapp.com/v2/quotes"));

//async é manipulador de rota
app.MapGet("/tarefas", async (AppDbContext db) => await db.Tarefas.ToArrayAsync());

app.MapGet("/tarefas/{id}", async (int id, AppDbContext db) => await db.Tarefas.FindAsync(id) is Tarefas tarefas ? Results.Ok(tarefas) : Results.NotFound());

app.MapGet("/tarefas/concluidas", async (AppDbContext db) => await db.Tarefas.Where(x => x.IsConcluida).ToListAsync());


//Método para deletar
app.MapDelete("/tarefas/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Tarefas.FindAsync(id) is Tarefas tarefas)
    {
        db.Remove(tarefas);
        await db.SaveChangesAsync();
        return Results.Ok(tarefas);

    }
    else
    {
        return Results.NotFound();
    }
});


//Método para atualizar
app.MapPut("/taredas/{id}", async (int id, Tarefas inputTarefas, AppDbContext db) =>
{ 
    var tarefa = await db.Tarefas.FindAsync(id);
    if (tarefa is null) return Results.NotFound();

    tarefa.Nome = inputTarefas.Nome;
    tarefa.IsConcluida = inputTarefas.IsConcluida;

    await db.SaveChangesAsync();
    return Results.NoContent();

});


//Método para inserir.
app.MapPost("/tarefas", async (Tarefas _terefas, AppDbContext db) =>
{
    db.Tarefas.Add(_terefas);
    await db.SaveChangesAsync();
    return Results.Created($"Tarefa inserida{_terefas.Id}", _terefas);
});


app.Run();





class Tarefas
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public bool IsConcluida { get; set; }
}

class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
    public DbSet<Tarefas> Tarefas => Set <Tarefas>();
}