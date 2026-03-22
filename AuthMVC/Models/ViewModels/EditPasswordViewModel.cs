using System.ComponentModel.DataAnnotations;

namespace AuthMVC.Models.ViewModels;

public class EditPasswordViewModel
{
    [Display(Name = "Eski Şifre")]
    public string OldPassword { get; set; }

    [Display(Name = "Yeni Şifre")]
    public string NewPassword { get; set; }
}
