namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEndofAppointment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "EndOfAppointment", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "EndOfAppointment");
        }
    }
}
