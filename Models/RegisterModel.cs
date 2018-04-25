using System.ComponentModel.DataAnnotations;

namespace PIMS.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 9)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Confirm Email")]
        [Compare("Email", ErrorMessage = "The emails you've entered do not match.")]
        public string ConfirmEmail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]   
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}