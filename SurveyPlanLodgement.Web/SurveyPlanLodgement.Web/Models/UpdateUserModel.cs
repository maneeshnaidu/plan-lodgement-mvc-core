using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Models
{
    public class UpdateUserModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please enter user first name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter user email")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Please enter default user password")]
        //[Compare("ConfirmPassword", ErrorMessage = "Password does not match")]
        //[Display(Name = "Password")]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        //[Required(ErrorMessage = "Please confirm default password")]
        //[Display(Name = "Confirm Password")]
        //[DataType(DataType.Password)]
        //public string ConfirmPassword { get; set; }
    }
}
