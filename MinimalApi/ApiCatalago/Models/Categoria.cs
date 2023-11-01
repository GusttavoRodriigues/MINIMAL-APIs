using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ApiCatalago.Models;

public class Categoria
{
    public Categoria()
    {
        Produtos = new Collection<Produto>();
    }
    public int IdCategoria { get; set; }
    public  string? Nome { get; set; }
    public string? Descricao { get; set; }

    public ICollection<Produto>? Produtos { get; set; }
}
