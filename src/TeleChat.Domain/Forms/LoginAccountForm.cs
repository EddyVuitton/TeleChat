using System.ComponentModel.DataAnnotations;

namespace TeleChat.Domain.Forms;

public class LoginAccountForm
{
    [Required(ErrorMessage = "Pole jest wymagane...")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "Pole jest wymagane...")]
    public string Password { get; set; } = null!;
}