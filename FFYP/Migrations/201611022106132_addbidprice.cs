namespace FFYP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addbidprice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bidings", "BidPrice", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bidings", "BidPrice");
        }
    }
}
