using System.ComponentModel.DataAnnotations;

namespace Boilerplate.OAuth.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
