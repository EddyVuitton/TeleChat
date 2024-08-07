using System.ComponentModel.DataAnnotations;

namespace TeleChat.Domain.Entities;

public class ReceiverType
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
}