using System.ComponentModel.DataAnnotations;

namespace AnteyaSidOnContainers.Services.Identity.API.ViewModels.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
