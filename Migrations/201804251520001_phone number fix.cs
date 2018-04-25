namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class phonenumberfix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "ApplicantPhoneNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointments", "ApplicantPhoneNumber", c => c.String());
        }
    }
}
