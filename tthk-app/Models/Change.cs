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
        public Change()
        {
            DayOfWeek = Date.DayOfWeek;
        }

        public DayOfWeek DayOfWeek { get; }
        public DateTime Date { get; set; }
        public string Group { get; set; }
        public string Lesson { get; set; }
        public string Teacher { get; set; }
        public string Room { get; set; }

        public static List<Change> GetChangesList()
        {
            List<Change> changesList = new List<Change>();
            foreach (var changesRow in ParserEngine.ParseChanges())
            {
                Change change = new Change
                {
                    Date = DateTime.ParseExact(changesRow[1], "DD.MM.YYYY", CultureInfo.InvariantCulture),
                    Group = changesRow[2],
                    Lesson = changesRow[3],
                    Teacher = changesRow[4],
                    Room = changesRow[5]
                };
            }

            return changesList;
        }
    }
}