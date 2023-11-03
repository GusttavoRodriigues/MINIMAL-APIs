using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    [JsonIgnore]
    public ICollection<Produto>? Produtos { get; set; }
}
