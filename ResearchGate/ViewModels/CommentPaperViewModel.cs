using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ResearchGate.Models;

namespace ResearchGate.ViewModels
{
    public class CommentPaperViewModel
    {
        public IEnumerable<Comment> Comment { get; set; }
        public Paper Paper { get; set; }
        public IEnumerable<Author> Authors { get; set; }
    }
}