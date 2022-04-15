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
       

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaperId { get; set; }
        [Required]
        public string PaperName { get; set; }

        public string PaperDescription { get; set; }

        public string ContentType { get; set; }

        public byte[] Data { get; set; }

        //public virtual ICollection<Author> Authors { get; set; }

        public virtual ICollection<AuthorPapers> AuthorPapers { get; set; }
    }
}