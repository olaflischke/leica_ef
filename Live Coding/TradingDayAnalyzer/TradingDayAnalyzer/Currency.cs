namespace TradingDayAnalyzer
{
    public class Currency
    {
        public string Symbol { get; set; }
        public double EuroValue { get; set; }

        public TradingDay TradingDay { get; set; }
        public int Id { get; set; }
    }
}