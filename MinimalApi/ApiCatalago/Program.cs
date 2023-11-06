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

app.MapPost("/Produtos",async(Produto produto,AppDbContext db) =>
{
    if (produto is null)
            return Results.NoContent();

    db.Produtos.Add(produto);
    await db.SaveChangesAsync();
    return Results.Created($"/Produtos/{produto.IdProduto}", produto);

});

app.MapPut("/Produtos/{id:int}",async(int id,Produto produto,AppDbContext db) =>
{
    if (produto.IdProduto != id)
        return Results.NotFound();
    var _produto = await db.Produtos.FindAsync(id);
    if(produto is null)
        return Results.NoContent();
    _produto.Nome = produto.Nome;
    _produto.Descricao = produto.Descricao;
    _produto.ImagemUrl = produto.ImagemUrl;
    _produto.Preco = produto.Preco;
    _produto.Estoque = produto.Estoque;
    _produto.DataCadastro = produto.DataCadastro;

    db.SaveChangesAsync();
    return Results.Ok(produto);

});

app.MapDelete("/Produtos/{id:int}",async(int id, AppDbContext db) =>
{
    var _produto = await db.Produtos.FindAsync(id);
    if (_produto is null)
        return Results.NotFound();
    db.Remove(_produto);
    await db.SaveChangesAsync();
    return Results.Ok(_produto);

});

#endregion




if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();
