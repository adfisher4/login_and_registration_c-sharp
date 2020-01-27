using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginAndRegistration.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set;}
        [Required]
        [MinLength(2, ErrorMessage="First name must be at least 2 characters or longer!")]
        [Display(Name = "First Name")]

        public string FirstName {get; set;}
        
        [Required]
        [MinLength(2, ErrorMessage="Last name must be at least 2 characters or longer!")]
        [Display(Name = "Last Name")]
        public string LastName {get; set;}

        [Required]
      
        [EmailAddress]
        public string Email {get; set;}
        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string Password {get; set;}
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;

        [NotMapped]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string Confirm {get; set;}

    }
}