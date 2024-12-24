using System.ComponentModel.DataAnnotations;

namespace CmsUI.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(40,MinimumLength =8,ErrorMessage ="The {0} must be at {2} and max {1} characters.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword",ErrorMessage ="Password does not match.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
