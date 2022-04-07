using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingDayAnalyzer
{
    public class TradingDayContext : DbContext
    {
        public TradingDayContext()//:base("name=TradingDayDbContext")
        {

        }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<TradingDay> TradingDays { get; set; }
    }
}
