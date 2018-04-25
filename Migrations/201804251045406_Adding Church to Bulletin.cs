namespace PIMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingChurchtoBulletin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bulletins", "ChurchId", c => c.Int(nullable: false));
            CreateIndex("dbo.Bulletins", "ChurchId");
            AddForeignKey("dbo.Bulletins", "ChurchId", "dbo.Church", "ChurchId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bulletins", "ChurchId", "dbo.Church");
            DropIndex("dbo.Bulletins", new[] { "ChurchId" });
            DropColumn("dbo.Bulletins", "ChurchId");
        }
    }
}
