﻿namespace TeleChat.Domain.Models.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}