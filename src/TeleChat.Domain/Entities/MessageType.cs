using System.ComponentModel.DataAnnotations;

namespace TeleChat.Domain.Entities;

public class MessageType
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string DefaultStyle { get; set; } = null!;
}