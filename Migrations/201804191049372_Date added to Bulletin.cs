namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateaddedtoBulletin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bulletins", "DateOfBulletin", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bulletins", "DateOfBulletin");
        }
    }
}
