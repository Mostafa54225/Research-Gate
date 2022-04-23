namespace ResearchGate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LikesModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorId = c.Int(nullable: false),
                        PaperId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Papers", t => t.PaperId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.PaperId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Likes", "PaperId", "dbo.Papers");
            DropForeignKey("dbo.Likes", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Likes", new[] { "PaperId" });
            DropIndex("dbo.Likes", new[] { "AuthorId" });
            DropTable("dbo.Likes");
        }
    }
}
