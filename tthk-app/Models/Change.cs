using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using FSharp.Data.Runtime;
using tthk_app.ParsingService;

namespace tthk_app.Models
{
    public class Change
    {
        List<string> stopList = new List<string>() {"Kuupäev", "Õpetaja", "Ruum", "Tund", "Õpetaja"};

        public DayOfWeek DayOfWeek { get; }

        public DateTime Date
        {
            get => this.Date;
            set => this.Date = value;
        }

        string Group { get; set; }
        string Lesson { get; set; }
        string Teacher { get; set; }
        string Room { get; set; }

        public Change()
        {
            DayOfWeek = Date.DayOfWeek;
            List<Change> changesList = new List<Change>();
            var changeRows = ParserEngine.ParseChanges();
            if (changeRows.Count > 0)
            {
                foreach (var changesRow in changeRows)
                {
                    Date = DateTime.ParseExact(changesRow[1], "DD.MM.YYYY", CultureInfo.InvariantCulture);
                    Group = changesRow[2];
                    Lesson = changesRow[3];
                    Teacher = changesRow[4];
                    Room = changesRow[5];
                    changesList.Add(this);
                }
            }
            else
            {
            }
        }
    }
}