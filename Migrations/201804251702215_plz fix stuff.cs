namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class plzfixstuff : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Record", "Document", c => c.Binary());
            DropColumn("dbo.Invoice", "ImageFileName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Invoice", "ImageFileName", c => c.String());
            AlterColumn("dbo.Record", "Document", c => c.Binary(nullable: false));
        }
    }
}
