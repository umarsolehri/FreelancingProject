namespace FFYP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sesemsteradd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SiteUsers", "Semester", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SiteUsers", "Semester");
        }
    }
}
