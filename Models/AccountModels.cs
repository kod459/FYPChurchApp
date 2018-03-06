using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using System.Web.Mvc;
using PIMS.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PIMS.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("PhotocopierDBContext")
        {
        }

            public DbSet<UserProfile> UserProfiles { get; set; }
            //public DbSet<MeterReading> MeterReading { get; set; }
            //public DbSet<PhotocopierDetails> PhotocopierDetails { get; set; }

            //protected override void OnModelCreating(DbModelBuilder modelBuilder)
            //{
            //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //}

    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.CompareAttribute("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

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

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }

    //added code
    public class CheckTheBox
    {
        public string state { get; set; }
        public IEnumerable<SelectListItem> states { get; set; }

        CheckTheBox()
        {
            states = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Selected = false,
                    Text = "Active",
                    Value = "Active"
                },
                new SelectListItem
                {
                    Selected = true,
                    Text = "Obsolete",
                    Value = "Obsolete"
                }
            };
        }
    }

    //more added code
    public class BlockedIPViewModel
    {
        public string IP { get; set; }
        public int ID { get; set; }
        public bool checkedd { get; set; }
    }

    
}
