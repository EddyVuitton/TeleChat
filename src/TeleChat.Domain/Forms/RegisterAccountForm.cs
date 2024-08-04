using System.ComponentModel.DataAnnotations;

namespace TeleChat.Domain.Forms;

public class RegisterAccountForm
{
    [Required(ErrorMessage = "Pole nie może być puste..."), StringLength(100, ErrorMessage = "Zbyt długa nazwa...")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Pole nie może być puste..."), StringLength(100, ErrorMessage = "Zbyt długa nazwa...")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "Pole nie może być puste...")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Pole nie może być puste..."), Compare(nameof(Password), ErrorMessage = "Podane hasła nie są identyczne...")]
    public string Password2 { get; set; } = null!;
}