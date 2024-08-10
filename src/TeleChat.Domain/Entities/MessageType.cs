using System.ComponentModel.DataAnnotations;

namespace TeleChat.Domain.Entities;

public class MessageType
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? DefaultStyle { get; set; }
}