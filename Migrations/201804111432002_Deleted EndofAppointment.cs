namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletedEndofAppointment : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Appointments", "EndOfAppointment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointments", "EndOfAppointment", c => c.DateTime(nullable: false));
        }
    }
}
