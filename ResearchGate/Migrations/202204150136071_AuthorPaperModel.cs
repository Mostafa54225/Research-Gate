namespace ResearchGate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthorPaperModel : DbMigration
    {
        public override void Up()
        {
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
            
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        AuthorId = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 450),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Username = c.String(nullable: false, maxLength: 450),
                        Password = c.String(nullable: false),
                        Salt = c.String(),
                        University = c.String(nullable: false),
                        Department = c.String(),
                        Mobile = c.String(nullable: false),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.AuthorId)
                .Index(t => t.Email, unique: true)
                .Index(t => t.Username, unique: true);
            
            CreateTable(
                "dbo.Papers",
                c => new
                    {
                        PaperId = c.Int(nullable: false, identity: true),
                        PaperName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PaperId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthorPapers", "PaperId", "dbo.Papers");
            DropForeignKey("dbo.AuthorPapers", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Authors", new[] { "Username" });
            DropIndex("dbo.Authors", new[] { "Email" });
            DropIndex("dbo.AuthorPapers", new[] { "PaperId" });
            DropIndex("dbo.AuthorPapers", new[] { "AuthorId" });
            DropTable("dbo.Papers");
            DropTable("dbo.Authors");
            DropTable("dbo.AuthorPapers");
        }
    }
}
