namespace ResearchGate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        CommentContent = c.String(nullable: false),
                        PaperId = c.Int(nullable: false),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Papers", t => t.PaperId, cascadeDelete: true)
                .Index(t => t.PaperId)
                .Index(t => t.AuthorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "PaperId", "dbo.Papers");
            DropForeignKey("dbo.Comments", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Comments", new[] { "AuthorId" });
            DropIndex("dbo.Comments", new[] { "PaperId" });
            DropTable("dbo.Comments");
        }
    }
}
