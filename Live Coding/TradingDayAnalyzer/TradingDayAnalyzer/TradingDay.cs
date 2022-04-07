using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace TradingDayAnalyzer
{
    public class TradingDay
    {
        public TradingDay(XElement tradingDayNode)
        {
            this.Date = Convert.ToDateTime(tradingDayNode.Attribute("time").Value);

            NumberFormatInfo nfiEcb = new NumberFormatInfo() { NumberDecimalSeparator = "." };

            this.Currencies = tradingDayNode.Elements().Select(el => new Currency()
                                                                    {
                                                                        Symbol = el.Attribute("currency").Value,
                                                                        EuroValue = Convert.ToDouble(el.Attribute("rate").Value, nfiEcb),
                                                                        TradingDay = this
                                                                    }).ToList();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<Currency> Currencies { get; set; }
        public string Location { get; set; }

    }
}