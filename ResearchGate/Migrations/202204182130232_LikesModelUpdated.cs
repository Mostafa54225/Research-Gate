namespace ResearchGate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LikesModelUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Likes", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.Permissions", "Status", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Permissions", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.Likes", "Status");
        }
    }
}
