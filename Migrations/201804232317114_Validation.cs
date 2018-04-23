namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Validation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "NameOfApplicant", c => c.String(nullable: false));
            AlterColumn("dbo.Appointments", "ApplicantPhoneNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Appointments", "ApplicantEmail", c => c.String(nullable: false));
            AlterColumn("dbo.Volunteer", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Volunteer", "Username", c => c.String(nullable: false));
            AlterColumn("dbo.Volunteer", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Volunteer", "VolunteerPhoneNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Bulletins", "DetailsOfBulletin", c => c.String(nullable: false));
            AlterColumn("dbo.House", "AddressLine1", c => c.String(nullable: false));
            AlterColumn("dbo.House", "AddressLine2", c => c.String(nullable: false));
            AlterColumn("dbo.Invoice", "Company", c => c.String(nullable: false));
            AlterColumn("dbo.Invoice", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Invoice", "PictureOfInvoice", c => c.Binary(nullable: false));
            AlterColumn("dbo.Invoice", "DateReceived", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Record", "NameOnRecord", c => c.String(nullable: false));
            AlterColumn("dbo.Record", "DocumentType", c => c.String(nullable: false));
            AlterColumn("dbo.Record", "Document", c => c.Binary(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Record", "Document", c => c.Binary());
            AlterColumn("dbo.Record", "DocumentType", c => c.String());
            AlterColumn("dbo.Record", "NameOnRecord", c => c.String());
            AlterColumn("dbo.Invoice", "DateReceived", c => c.DateTime());
            AlterColumn("dbo.Invoice", "PictureOfInvoice", c => c.Binary());
            AlterColumn("dbo.Invoice", "Description", c => c.String());
            AlterColumn("dbo.Invoice", "Company", c => c.String());
            AlterColumn("dbo.House", "AddressLine2", c => c.String());
            AlterColumn("dbo.House", "AddressLine1", c => c.String());
            AlterColumn("dbo.Bulletins", "DetailsOfBulletin", c => c.String());
            AlterColumn("dbo.Volunteer", "VolunteerPhoneNumber", c => c.String());
            AlterColumn("dbo.Volunteer", "Email", c => c.String());
            AlterColumn("dbo.Volunteer", "Username", c => c.String());
            AlterColumn("dbo.Volunteer", "Name", c => c.String());
            AlterColumn("dbo.Appointments", "ApplicantEmail", c => c.String());
            AlterColumn("dbo.Appointments", "ApplicantPhoneNumber", c => c.String());
            AlterColumn("dbo.Appointments", "NameOfApplicant", c => c.String());
        }
    }
}
