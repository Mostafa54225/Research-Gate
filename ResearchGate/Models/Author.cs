using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ResearchGate.Models
{
    public class Author
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId { get; set; }


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


        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Username { get; set; }


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
        [DefaultValue("General")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Number is required")]
        [Display(Name = "Number")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }


        public byte[] Image { get; set; }


        
        public virtual ICollection<AuthorPapers> AuthorPapers { get; set; }
    }
}