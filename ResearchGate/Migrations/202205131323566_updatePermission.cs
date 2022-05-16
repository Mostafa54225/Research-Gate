namespace ResearchGate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePermission : DbMigration
    {
        public override void Up()
        {
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
        }
    }
}
