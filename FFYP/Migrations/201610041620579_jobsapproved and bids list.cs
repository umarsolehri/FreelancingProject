namespace FFYP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jobsapprovedandbidslist : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BidsLists",
                c => new
                    {
                        BidsListID = c.Int(nullable: false, identity: true),
                        MarkasRead = c.Boolean(nullable: false),
                        ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BidsListID)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.JobApproveds",
                c => new
                    {
                        JobApprovedID = c.Int(nullable: false, identity: true),
                        MarkasRead = c.Boolean(nullable: false),
                        SiteUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobApprovedID)
                .ForeignKey("dbo.SiteUsers", t => t.SiteUserID, cascadeDelete: true)
                .Index(t => t.SiteUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobApproveds", "SiteUserID", "dbo.SiteUsers");
            DropForeignKey("dbo.BidsLists", "ProjectID", "dbo.Projects");
            DropIndex("dbo.JobApproveds", new[] { "SiteUserID" });
            DropIndex("dbo.BidsLists", new[] { "ProjectID" });
            DropTable("dbo.JobApproveds");
            DropTable("dbo.BidsLists");
        }
    }
}
