using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ResearchGate.Models
{
    public class AuthorPapers
    {

        [Key]
        [Column(Order =1)]
        public int AuthorId { get; set; }

        [Key]
        [Column(Order =2)]
        public int PaperId { get; set; }

        public virtual Author Author { get; set; }
        public virtual Paper Paper { get; set; }
    }
}