namespace ResearchGate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateImageColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "ImagePath", c => c.String());
            DropColumn("dbo.Authors", "ProfileImage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Authors", "ProfileImage", c => c.String());
            DropColumn("dbo.Authors", "ImagePath");
        }
    }
}
