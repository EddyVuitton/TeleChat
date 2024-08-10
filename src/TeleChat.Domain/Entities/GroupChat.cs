using System.ComponentModel.DataAnnotations;

namespace TeleChat.Domain.Entities;

public class GroupChat
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public Guid Guid { get; set; } = Guid.NewGuid();
    [Required]
    public DateTime Created { get; set; } = DateTime.UtcNow;
}