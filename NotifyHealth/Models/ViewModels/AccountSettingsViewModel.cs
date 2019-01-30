using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;

namespace NotifyHealth.Models.ViewModels
{
    public class AccountSettingsViewModel
    {
        public int UserID { get; set; }
        public int UserLogonID { get; set; }

        [Display(Name = "Logon Name")]
        public string LogonName { get; set; }

        public int OrganizationID { get; set; }

        public string Title { get; set; }

        [Required]
        public string Forename { get; set; }

        [Required]
        public string Surname { get; set; }

        public string JobTitle { get; set; }

        [Display(Name = "Telephone")]
        public string WorkTelephoneNo { get; set; }

        [Required]
        [Display(Name = "Hint Question")]
        public virtual int HintQuestionID { get; set; }

        [Display(Name = "Hint Question")]
        public virtual IEnumerable<SelectListItem> HintQuestion { get; set; }

        [Required]
        [Display(Name = "Hint Answer")]
        public string HintAnswer { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Repeat New Password")]
        public string CheckPassword { get; set; }

        [NotMapped]
        public string MustChangePwd { get; set; }

        public int UserRole { get; set; }

        public HttpPostedFileBase PhotoFile { get; set; }
        public string PhotoPath { get; set; }
    }

    public class PasswordRecoveryGetUserName
    {
        [Required]
        [Display(Name = "Enter your Username")]
        public string Username { get; set; }
    }

    public class PasswordRecovery
    {
        public string Username { get; set; }

        [Display(Name = "Hint Question")]
        public string HintQuestion { get; set; }

        [Required]
        [Display(Name = "Hint Answer")]
        public string HintAnswer { get; set; }
    }
}