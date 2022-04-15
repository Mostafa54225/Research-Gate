namespace ResearchGate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePaperTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Papers", "PaperDescription", c => c.String());
            AddColumn("dbo.Papers", "ContentType", c => c.String());
            AddColumn("dbo.Papers", "Data", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Papers", "Data");
            DropColumn("dbo.Papers", "ContentType");
            DropColumn("dbo.Papers", "PaperDescription");
        }
    }
}
