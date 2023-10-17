using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSXImport.Portal.Models
{
    public class Login
    {
        public string RedirectURL { get; set; }
        public bool IsRedirect { get; set; }
        public string auth_code { get; set; }
        [Required(ErrorMessage = "Please Enter Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }
    }
}
