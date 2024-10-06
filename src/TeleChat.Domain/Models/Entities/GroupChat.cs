namespace TeleChat.Domain.Models.Entities;

public class GroupChat
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid Guid { get; set; } = Guid.NewGuid();
    public DateTime Created { get; set; } = DateTime.Now;
}