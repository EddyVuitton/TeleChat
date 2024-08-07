using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeleChat.Domain.Entities;

public class Message
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Text { get; set; } = null!;
    [ForeignKey("MessageType")]
    public int TypeId { get; set; }
    [ForeignKey("MessageType")]
    public int ReceiverId { get; set; }
}