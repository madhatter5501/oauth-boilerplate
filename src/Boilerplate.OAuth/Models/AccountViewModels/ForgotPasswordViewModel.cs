using System.ComponentModel.DataAnnotations;

namespace Boilerplate.OAuth.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
