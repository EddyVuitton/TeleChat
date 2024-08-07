using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeleChat.Domain.Entities;

public class Receiver
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [ForeignKey("ReceiverType")]
    public int TypeId { get; set; }
}