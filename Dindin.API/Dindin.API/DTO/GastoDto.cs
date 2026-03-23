using System.ComponentModel.DataAnnotations;
using Dindin.API.Models;

namespace Dindin.API.DTO;

public class InserirGastoDto
{
    [Required]
    [MaxLength(200)]
    public string Descricao { get; set; }
    
    [Required]
    public decimal Quantidade { get; set; }
    
    [Required] 
    public MetodoPagamento MetodoPagamento { get; set; }

    [MaxLength(120)] 
    public string? Categoria { get; set; }

    public int? GrupoId { get; set; }
}

public class DeletarGastoDto
{
    [Required]
    public int Id { get; set; }
    
    public int? GroupId { get; set; }
}

public class AtualizarGastoDto
{
    [MaxLength(200)]
    public string? Descricao { get; set; }
    
    public decimal? Quantidade { get; set; }
    
    public MetodoPagamento? MetodoPagamento { get; set; }

    [MaxLength(120)] 
    public string? Categoria { get; set; }
    
    [Required] 
    public int Id { get; set; }
}

public class ObterGastosDto
{
    public int? GroupId { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
}