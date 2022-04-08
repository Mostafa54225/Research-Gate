using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ResearchGate.Models
{
    public class Paper
    {
        public Paper()
        {
            this.Authors = new HashSet<Author>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaperId { get; set; }
        [Required]
        public string PaperName { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
    }
}