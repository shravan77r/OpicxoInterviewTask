using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SSXImport.WebAPI.Models
{
    public class Authentication
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
