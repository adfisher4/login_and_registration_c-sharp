using System.ComponentModel.DataAnnotations;

namespace LoginAndRegistration.Models
{
    public class LoginUser
    {
        public string Email {get; set;}
        // [Required(ErrorMessage="Enter your password")]
        [DataType(DataType.Password)]

        public string Password { get; set; }
    }
}