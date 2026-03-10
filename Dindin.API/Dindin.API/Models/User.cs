﻿using System.ComponentModel.DataAnnotations;

namespace Dindin.API.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(64)]
    public string PasswordHash { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Gastos> Gastos { get; set; } = new List<Gastos>();
    public ICollection<Depositos> Depositos { get; set; } = new List<Depositos>();
    public ICollection<RefreshToken> RefreshTokens { get; set; }
    public ICollection<UserGroup> UserGroup { get; set; } = new List<UserGroup>();

}