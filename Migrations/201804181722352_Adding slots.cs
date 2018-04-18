namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingslots : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "Slots", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "Slots");
        }
    }
}
