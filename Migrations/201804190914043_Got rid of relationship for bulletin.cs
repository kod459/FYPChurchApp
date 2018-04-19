namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gotridofrelationshipforbulletin : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bulletins", "AdministrationId", "dbo.Administration");
            DropIndex("dbo.Bulletins", new[] { "AdministrationId" });
            AddColumn("dbo.Bulletins", "AdminPosting", c => c.String());
            DropColumn("dbo.Bulletins", "AdministrationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bulletins", "AdministrationId", c => c.Int(nullable: false));
            DropColumn("dbo.Bulletins", "AdminPosting");
            CreateIndex("dbo.Bulletins", "AdministrationId");
            AddForeignKey("dbo.Bulletins", "AdministrationId", "dbo.Administration", "AdministrationId", cascadeDelete: true);
        }
    }
}
