namespace FFYP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfacpro : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FavProes",
                c => new
                    {
                        FavProID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        Userid = c.String(),
                    })
                .PrimaryKey(t => t.FavProID)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FavProes", "ProjectID", "dbo.Projects");
            DropIndex("dbo.FavProes", new[] { "ProjectID" });
            DropTable("dbo.FavProes");
        }
    }
}
