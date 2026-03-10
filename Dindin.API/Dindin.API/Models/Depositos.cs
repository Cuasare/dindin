using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dindin.API.Models;

public enum MetodoDeposito
{
    Pix,
    Emprestimo,
    CartaoCredito,
    CartaoDebito,
    Salario,
    Outro,
}

public class Depositos
{
    public int Id { get; set; }

    [Required] [MaxLength(200)] public string Descricao { get; set; } = string.Empty;
    
    [Required] [Column(TypeName = "decimal(18,2)")] public decimal Quantidade { get; set; }
    
    [Required] public MetodoDeposito MetodoDeposito { get; set; }
    
    [MaxLength(200)] public string? Categoria { get; set; }

    public DateTime AddAt { get; set; } = DateTime.UtcNow;

    public int? GroupId { get; set; }
    public Group? Group { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;

}