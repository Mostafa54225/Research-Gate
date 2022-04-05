using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ResearchGate.Models
{
    public class Author
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [MinLength(6)]
        public string Password { get; set; }
        public string Salt { get; set; }



        [Required(ErrorMessage = "University name is required")]
        [Display(Name = "University")]
        public string University { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Number is required")]
        [Display(Name = "Number")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }


        public string ProfileImage { get; set; }
    }
}