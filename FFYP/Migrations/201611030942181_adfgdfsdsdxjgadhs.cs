namespace FFYP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adfgdfsdsdxjgadhs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SiteUsers", "IDCard", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SiteUsers", "IDCard");
        }
    }
}
