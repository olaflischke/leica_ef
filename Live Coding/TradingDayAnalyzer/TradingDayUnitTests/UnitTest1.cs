using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TradingDayAnalyzer;

namespace TradingDayUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IsArchiveInitializing()
        {
            Archive archive = new Archive("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist.xml");

            Assert.AreEqual(64, archive.TradingDays.Count);
        }

        [TestMethod]
        public void IsArchiveSaving()
        {
            Archive archive = new Archive("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml");

            archive.SaveToDb();

        }
    }
}
