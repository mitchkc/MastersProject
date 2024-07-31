using System.ComponentModel.DataAnnotations;

namespace MastersProject.Web.Models.User;
public class ForgotPasswordViewModel
{
    [Required]
    public string Email { get; set; }
    
}
