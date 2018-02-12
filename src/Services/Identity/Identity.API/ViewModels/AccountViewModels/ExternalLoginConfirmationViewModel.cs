using System.ComponentModel.DataAnnotations;

namespace AnteyaSidOnContainers.Services.Identity.API.ViewModels.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
