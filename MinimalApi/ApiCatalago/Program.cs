using ApiCatalago.Context;
using ApiCatalago.Models;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? sqlServerConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(sqlServerConnection));

var app = builder.Build();

#region ENDPOINTS CATEGORIAS
app.MapGet("/Categoria", async (AppDbContext db) =>{ return await db.Categorias.ToListAsync();});

app.MapGet("/Categoria/{id:int}", async (int id, AppDbContext db) => { 
    return await db.Categorias.FindAsync(id) is Categoria categoria ? Results.Ok(categoria) : Results.NotFound();
});

app.MapPost("/Categoria", async (Categoria categoria, AppDbContext db) =>
{
    if (categoria is null)
       return Results.NotFound("Erro ao inserir Categoria");

    db.Categorias.Add(categoria);
    await db.SaveChangesAsync();

    return Results.Created($"/Categoria/{categoria.IdCategoria}", categoria);
});

app.MapPut("/Categoria/{id:int}", async (int Id, Categoria categoria, AppDbContext db) =>
{
    if (categoria.IdCategoria !=Id)
        return Results.BadRequest();
    var categoriaDB = await db.Categorias.FindAsync(Id);
    if (categoriaDB is null) return Results.NotFound();

    categoriaDB.Nome = categoria.Nome;
    categoriaDB.Descricao = categoria.Descricao;

    db.SaveChangesAsync();
    return Results.Ok(categoriaDB); 
});

app.MapDelete("Categoria/{id:int}", async (int id, AppDbContext db) =>
{
    var CategoriaDb = await db.Categorias.FindAsync(id);
    if (CategoriaDb is null) return Results.NoContent();

    db.Categorias.Remove(CategoriaDb);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
#endregion

#region ENDPOINTS PRODUTOS
app.MapGet("/Produtos",async(AppDbContext db) => { return await db.Produtos.ToListAsync(); });
app.MapGet("/Produtos/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Produtos.FindAsync(id) is Produto produto ? Results.Ok(produto) : Results.NotFound();

});


//app.MapPost();
//app.MapPut();
//app.MapDelete();

#endregion




if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();
