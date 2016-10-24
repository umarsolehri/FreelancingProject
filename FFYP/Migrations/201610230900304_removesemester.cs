namespace FFYP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removesemester : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SiteUsers", "Semester");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SiteUsers", "Semester", c => c.String());
        }
    }
}
