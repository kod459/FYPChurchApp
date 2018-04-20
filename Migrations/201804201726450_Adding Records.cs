namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRecords : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Record", "NameOnRecord", c => c.String());
            AddColumn("dbo.Record", "Document", c => c.Binary());
            AlterColumn("dbo.Record", "UploadDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Record", "Name");
            DropColumn("dbo.Record", "Size");
            DropColumn("dbo.Record", "Version");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Record", "Version", c => c.Long());
            AddColumn("dbo.Record", "Size", c => c.Long());
            AddColumn("dbo.Record", "Name", c => c.String());
            AlterColumn("dbo.Record", "UploadDate", c => c.DateTime());
            DropColumn("dbo.Record", "Document");
            DropColumn("dbo.Record", "NameOnRecord");
        }
    }
}
