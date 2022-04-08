namespace ResearchGate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaperTableAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Papers",
                c => new
                    {
                        PaperId = c.Int(nullable: false, identity: true),
                        PaperName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PaperId);
            
            CreateTable(
                "dbo.AuthorPapers",
                c => new
                    {
                        AuthorId = c.Int(nullable: false),
                        PaperId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AuthorId, t.PaperId })
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Papers", t => t.PaperId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.PaperId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthorPapers", "PaperId", "dbo.Papers");
            DropForeignKey("dbo.AuthorPapers", "AuthorId", "dbo.Authors");
            DropIndex("dbo.AuthorPapers", new[] { "PaperId" });
            DropIndex("dbo.AuthorPapers", new[] { "AuthorId" });
            DropTable("dbo.AuthorPapers");
            DropTable("dbo.Papers");
        }
    }
}
