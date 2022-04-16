namespace ResearchGate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedByCol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuthorPapers", "CreatedBy", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AuthorPapers", "CreatedBy");
        }
    }
}
