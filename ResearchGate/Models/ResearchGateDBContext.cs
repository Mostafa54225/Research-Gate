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
        public DbSet<Paper> Papers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany<Paper>(p => p.Papers)
                .WithMany(a => a.Authors)
                .Map(ap =>
                {
                    ap.MapLeftKey("AuthorId");
                    ap.MapRightKey("PaperId");
                    ap.ToTable("AuthorPapers");
                });
        }
    }
}

