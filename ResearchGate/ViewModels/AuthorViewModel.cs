using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ResearchGate.Models;
namespace ResearchGate.ViewModels
{
    public class AuthorViewModel
    {
        public Author Author { get; set; }
        public IEnumerable<AuthorPapers> AuthorPapers { get; set; }
    }
}