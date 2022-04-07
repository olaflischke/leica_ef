namespace TradingDayAnalyzer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocationAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradingDays", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradingDays", "Location");
        }
    }
}
