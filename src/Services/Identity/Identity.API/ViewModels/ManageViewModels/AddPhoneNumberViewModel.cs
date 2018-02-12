using System.ComponentModel.DataAnnotations;

namespace AnteyaSidOnContainers.Services.Identity.API.ViewModels.ManageViewModels
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
