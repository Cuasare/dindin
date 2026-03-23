using System.ComponentModel.DataAnnotations;

namespace Dindin.API.DTO;

public class CreateGroupDto
{
    [Required]
    public string Nome { get; set; }
}