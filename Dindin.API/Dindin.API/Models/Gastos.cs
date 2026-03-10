using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dindin.API.Models;

public enum MetodoPagamento
{
    Pix,
    CartaoCredito,
    CartaoDebito,
    Dinheiro,
    Outro,
}

public class Gastos
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Descricao { get; set; } = string.Empty;
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantidade { get; set; }
    
    [Required]
    public MetodoPagamento MetodoPagamento { get; set; }
    
    [MaxLength(120)] public string? Categoria { get; set; }

    public DateTime AddAt { get; set; } = DateTime.UtcNow;

    public int? GrupoId { get; set; }
    public Group? Group { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

}