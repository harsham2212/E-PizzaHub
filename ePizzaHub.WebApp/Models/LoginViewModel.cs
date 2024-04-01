using System.ComponentModel.DataAnnotations;

namespace ePizzaHub.WebApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please Enter Email")]
        [EmailAddress(ErrorMessage ="Please Enter a Valid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please Enter Password")]
        public string Password { get; set; }
    }
}
