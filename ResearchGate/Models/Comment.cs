using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ResearchGate.Models
{
    public class Comment
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        [Required(ErrorMessage = "Comment Content is Required")]
        [MinLength(3)]
        public string CommentContent { get; set; }

        public virtual Paper Paper { get; set; }

        public int PaperId { get; set; }

        public virtual Author Author { get; set; }

        public int AuthorId { get; set; }
    }
}