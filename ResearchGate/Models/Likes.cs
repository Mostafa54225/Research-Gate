using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ResearchGate.Models;
namespace ResearchGate.Models
{
    public class Likes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Status { get; set; }

        public int AuthorId { get; set; }
        public int PaperId { get; set; }

        public virtual Paper Paper { get; set; }
        public virtual Author Author { get; set; }
    }
}