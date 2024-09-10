using System.ComponentModel.DataAnnotations;

namespace TeleChat.Domain.Forms;

public class AddGroupForm
{
    [Required(ErrorMessage = "Nazwa grupy nie może być pusta...")]
    public string Name { get; set; } = null!;
}