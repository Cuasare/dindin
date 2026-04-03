using System.ComponentModel.DataAnnotations;

namespace Dindin.API.DTO;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(8)]
    public string Password { get; set; }

    [Required] 
    public string Name { get; set; }
}

public class DeleteDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}

public class RefreshTokenDto
{
    [Required]
    public string RefreshToken { get; set; }
}
