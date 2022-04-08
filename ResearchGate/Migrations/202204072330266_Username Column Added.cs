namespace ResearchGate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsernameColumnAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "Username", c => c.String(nullable: false, maxLength: 450));
            CreateIndex("dbo.Authors", "Username", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Authors", new[] { "Username" });
            DropColumn("dbo.Authors", "Username");
        }
    }
}
