using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiCatalago.Models;

public class Produto
{
    public int IdProduto { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public string? ImagemUrl { get; set; }
    public DateTime DataCadastro { get; set; }

    public int IdCategoria { get; set; }
    public Categoria? Categoria { get; set; }
}
