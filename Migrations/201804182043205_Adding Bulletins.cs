namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingBulletins : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bulletins",
                c => new
                    {
                        BulletinsID = c.Int(nullable: false, identity: true),
                        DetailsOfBulletin = c.String(),
                        AdministrationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BulletinsID)
                .ForeignKey("dbo.Administration", t => t.AdministrationId, cascadeDelete: true)
                .Index(t => t.AdministrationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bulletins", "AdministrationId", "dbo.Administration");
            DropIndex("dbo.Bulletins", new[] { "AdministrationId" });
            DropTable("dbo.Bulletins");
        }
    }
}
