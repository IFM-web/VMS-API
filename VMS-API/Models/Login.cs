using System.ComponentModel.DataAnnotations;

namespace VMS_API.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
