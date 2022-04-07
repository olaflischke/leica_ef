namespace TradingDayAnalyzer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Symbol = c.String(),
                        EuroValue = c.Double(nullable: false),
                        TradingDay_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TradingDays", t => t.TradingDay_Id)
                .Index(t => t.TradingDay_Id);
            
            CreateTable(
                "dbo.TradingDays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Currencies", "TradingDay_Id", "dbo.TradingDays");
            DropIndex("dbo.Currencies", new[] { "TradingDay_Id" });
            DropTable("dbo.TradingDays");
            DropTable("dbo.Currencies");
        }
    }
}
