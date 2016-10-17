namespace FFYP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class descriptionrequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "Description", c => c.String());
        }
    }
}
