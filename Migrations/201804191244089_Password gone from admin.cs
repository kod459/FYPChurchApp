namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Passwordgonefromadmin : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Administration", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Administration", "Password", c => c.String());
        }
    }
}
