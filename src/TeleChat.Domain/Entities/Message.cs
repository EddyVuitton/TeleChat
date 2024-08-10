﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeleChat.Domain.Entities;

public class Message
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Text { get; set; } = null!;
    [ForeignKey(nameof(MessageType))]
    public int TypeId { get; set; }
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    [ForeignKey(nameof(GroupChat))]
    public int GroupChatId { get; set; }
    [Required]
    public DateTime Created { get; set; } = DateTime.UtcNow;

    public virtual MessageType MessageType { get; set; } = new();
    public virtual User User { get; set; } = new();
    public virtual GroupChat GroupChat { get; set; } = new();
}