using System.ComponentModel.DataAnnotations;

namespace Dindin.API.DTO;

public class createGroupDTos
{
    [Required]
    public string Nome { get; set; }
}