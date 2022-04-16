using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ResearchGate.Models
{
    public class Permissions
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SenderId { get; set; }

        public int AuthorId { get; set; }

        public int PaperId { get; set; }

        public int Status { get; set; } // 0 => Pending  1 => Approved   -1 => Disapproved
    }
}