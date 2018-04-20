namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRecordswithoutAdminID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Record", "AdministrationId", "dbo.Administration");
            DropIndex("dbo.Record", new[] { "AdministrationId" });
            AddColumn("dbo.Record", "UploadedBy", c => c.String());
            DropColumn("dbo.Record", "AdministrationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Record", "AdministrationId", c => c.Int(nullable: false));
            DropColumn("dbo.Record", "UploadedBy");
            CreateIndex("dbo.Record", "AdministrationId");
            AddForeignKey("dbo.Record", "AdministrationId", "dbo.Administration", "AdministrationId", cascadeDelete: true);
        }
    }
}
