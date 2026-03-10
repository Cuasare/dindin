using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dindin.API.Models;

namespace Dindin.API.DTO;

public class inserirDepositoDTOs
{
    [Required]
    [MaxLength(200)]
    public string Descricao { get; set; }
    
    [Required]
    public decimal Quantidade { get; set; }
    
    [Required] 
    public MetodoDeposito MetodoDeposito { get; set; }

    [MaxLength(120)] 
    public string? Categoria { get; set; }
}

public class deletarDepositoDTOs
{
    [Required]
    public int Id { get; set; }
}

public class atualizarDepositoDTOs
{
    [MaxLength(200)]
    public string? Descricao { get; set; }
    
    public decimal? Quantidade { get; set; }
    
    public MetodoDeposito? MetodoDeposito { get; set; }

    [MaxLength(120)] 
    public string? Categoria { get; set; }
    
    [Required] 
    public int Id { get; set; }
}

public class obterDepositosDTOs
{
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
}