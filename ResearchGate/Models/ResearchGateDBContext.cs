using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ResearchGate.Models
{
    public class ResearchGateDBContext: DbContext
    {
        public ResearchGateDBContext(): base("ConnectionDB")
        {

        }
        public DbSet<Author> Authors { get; set; }
    }
}

