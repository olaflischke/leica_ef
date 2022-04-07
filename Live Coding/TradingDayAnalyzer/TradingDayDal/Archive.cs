﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TradingDayDal
{
    public class Archive
    {
        public Archive(string url)
        {
            this.TradingDays = GetData(url);

            //SaveToDb();
        }

        public void SaveToDb()
        {
            TradingDayContext context = new TradingDayContext();

            // Datenbank löschen (fürs Debugging und Testing im frühen Stadium des Projekts)
            context.Database.EnsureDeleted();

            // Datenbank anlegen, wenn nicht vorhanden
            context.Database.EnsureCreated();

            context.TradingDays.AddRange(this.TradingDays);
            context.SaveChanges();

        }

        private List<TradingDay>? GetData(string url)
        {
            XDocument document = XDocument.Load(url);

            var q = document.Root.Descendants().Where(nd => nd.Name.LocalName == "Cube" && nd.Attributes().Any(at => at.Name == "time"))
                                            .Select(nd => new TradingDay(nd));

            return q.ToList();
        }

        public List<TradingDay> TradingDays { get; set; }
    }
}
