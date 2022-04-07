using System.Globalization;
using System.Xml.Linq;

namespace TradingDayDal
{
    public class TradingDay
    {
        public TradingDay()
        {

        }

        public TradingDay(XElement tradingDayNode)
        {
            this.Date = Convert.ToDateTime(tradingDayNode.Attribute("time").Value);

            NumberFormatInfo nfiEcb = new NumberFormatInfo() { NumberDecimalSeparator = "." };

            this.Currencies = tradingDayNode.Elements().Select(el => new Currency()
                                                                    {
                                                                        Symbol = el.Attribute("currency").Value,
                                                                        EuroValue = Convert.ToDouble(el.Attribute("rate").Value, nfiEcb),
                                                                        TradingDay=this
                                                                    }).ToList();
        }

        public DateTime Date { get; set; }
        public List<Currency> Currencies { get; set; }
        public int Id { get; set; }

    }
}