namespace ResearchGate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixImageColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "Image", c => c.Binary());
            DropColumn("dbo.Authors", "ImagePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Authors", "ImagePath", c => c.String());
            DropColumn("dbo.Authors", "Image");
        }
    }
}
