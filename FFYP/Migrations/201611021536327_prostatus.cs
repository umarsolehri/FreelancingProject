namespace FFYP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prostatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ProStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "ProStatus");
        }
    }
}
