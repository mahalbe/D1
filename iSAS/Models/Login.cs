using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ISas.Web.Models
{

    public class Login
    {
        public Login()
        {
            SessionList = new List<SelectListItem>();
        }

        [Required(ErrorMessage = "Enter Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [System.ComponentModel.DataAnnotations.Compare("password", ErrorMessage = "Invalid user name or password.")]
        public string password { get; set; }

        [Required(ErrorMessage = "Enter User name")]
        public string username { get; set; }

        public int userid { get; set; }

        //public string OldPassword { get; set; }
        //public string NewPasword { get; set; }
        //public string RetypeNewPassword { get; set; }
        //public string SessID { get; set; }
        //public string SessName { get; set; }

        public List<SelectListItem> SessionList { get; set; }
        public string SelectedSessionId { get; set; }
    }


}
