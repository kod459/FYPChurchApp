namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Morevalidationplzdontbreak : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Administration", "AdministratorName", c => c.String(nullable: false));
            AlterColumn("dbo.Administration", "AdminUsername", c => c.String(nullable: false));
            AlterColumn("dbo.Administration", "PhoneNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Administration", "EmailAddress", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Administration", "EmailAddress", c => c.String());
            AlterColumn("dbo.Administration", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Administration", "AdminUsername", c => c.String());
            AlterColumn("dbo.Administration", "AdministratorName", c => c.String());
        }
    }
}
