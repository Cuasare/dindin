using System.ComponentModel.DataAnnotations;
using Dindin.API.Models;

namespace Dindin.API.DTO;

public class inserirGastoDTOs
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

public class deletarGastoDTOs
{
    [Required]
    public int Id { get; set; }
}

public class atualizarGastoDTOs
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

public class obterGastos
{
    public int? GroupId { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
}