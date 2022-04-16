using ResearchGate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResearchGate.ViewModels
{
    public class AuthoPaperViewModel
    {
        public List<Author> Authors { get; set; }
        public List<Paper> Papers { get; set; }

    }
}