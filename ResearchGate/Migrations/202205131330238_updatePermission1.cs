namespace ResearchGate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePermission1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Permissions", "AuthorId", "dbo.Authors");
            DropForeignKey("dbo.Permissions", "PaperId", "dbo.Papers");
            DropForeignKey("dbo.Permissions", "SenderId", "dbo.Authors");
            DropIndex("dbo.Permissions", new[] { "SenderId" });
            DropIndex("dbo.Permissions", new[] { "AuthorId" });
            DropIndex("dbo.Permissions", new[] { "PaperId" });
            AlterColumn("dbo.Permissions", "SenderId", c => c.Int(nullable: false));
            AlterColumn("dbo.Permissions", "AuthorId", c => c.Int(nullable: false));
            AlterColumn("dbo.Permissions", "PaperId", c => c.Int(nullable: false));
            CreateIndex("dbo.Permissions", "SenderId");
            CreateIndex("dbo.Permissions", "AuthorId");
            CreateIndex("dbo.Permissions", "PaperId");
            AddForeignKey("dbo.Permissions", "AuthorId", "dbo.Authors", "AuthorId", cascadeDelete: false);
            AddForeignKey("dbo.Permissions", "PaperId", "dbo.Papers", "PaperId", cascadeDelete: false);
            AddForeignKey("dbo.Permissions", "SenderId", "dbo.Authors", "AuthorId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Permissions", "SenderId", "dbo.Authors");
            DropForeignKey("dbo.Permissions", "PaperId", "dbo.Papers");
            DropForeignKey("dbo.Permissions", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Permissions", new[] { "PaperId" });
            DropIndex("dbo.Permissions", new[] { "AuthorId" });
            DropIndex("dbo.Permissions", new[] { "SenderId" });
            AlterColumn("dbo.Permissions", "PaperId", c => c.Int());
            AlterColumn("dbo.Permissions", "AuthorId", c => c.Int());
            AlterColumn("dbo.Permissions", "SenderId", c => c.Int());
            CreateIndex("dbo.Permissions", "PaperId");
            CreateIndex("dbo.Permissions", "AuthorId");
            CreateIndex("dbo.Permissions", "SenderId");
            AddForeignKey("dbo.Permissions", "SenderId", "dbo.Authors", "AuthorId");
            AddForeignKey("dbo.Permissions", "PaperId", "dbo.Papers", "PaperId");
            AddForeignKey("dbo.Permissions", "AuthorId", "dbo.Authors", "AuthorId");
        }
    }
}
