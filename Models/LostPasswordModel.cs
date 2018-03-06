using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PIMS.Models
{
    public class LostPasswordModel
    {
        [Required(ErrorMessage = "We need your email to send you a reset link!")]
        [Display(Name = "Your account email")]
        [EmailAddress(ErrorMessage = "Not a valid email--Please enter a valid address to reset password.")]
        public string Email { get; set; }
    }
}