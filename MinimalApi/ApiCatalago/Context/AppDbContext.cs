using ApiCatalago.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Categoria>? Categorias { get; set; }
    public DbSet<Produto>? Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        //Categoria
        mb.Entity<Categoria>().HasKey(x => x.IdCategoria);
        mb.Entity<Categoria>().Property(x => x.Nome).HasMaxLength(100).IsRequired();
        mb.Entity<Categoria>().Property(x => x.Descricao).HasMaxLength(300).IsRequired();

        //Produto
        mb.Entity<Produto>().HasKey(x => x.IdProduto);
        mb.Entity<Produto>().Property(x =>x.Nome).HasMaxLength(100).IsRequired();
        mb.Entity<Produto>().Property(x => x.Descricao).HasMaxLength(100).IsRequired();
        mb.Entity<Produto>().Property(x => x.ImagemUrl).HasMaxLength(100).IsRequired();
        mb.Entity<Produto>().Property(x => x.Preco).HasPrecision(14, 2);


        //Relacionamento
        mb.Entity<Produto>().
                    HasOne<Categoria>(x => x.Categoria)
                        .WithMany(x => x.Produtos)
                            .HasForeignKey(x => x.IdCategoria);


    }
}
