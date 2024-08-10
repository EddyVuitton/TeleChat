﻿using System.ComponentModel.DataAnnotations;

namespace TeleChat.Domain.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Login { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public DateTime Created { get; set; } = DateTime.UtcNow;
    [Required]
    public bool IsActive { get; set; }
}