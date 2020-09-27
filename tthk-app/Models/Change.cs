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
        private List<Change> changesList;
        List<string> stopList = new List<string>() {"Kuupäev", "Õpetaja", "Ruum", "Tund", "Õpetaja"};

        internal DayOfWeek DayOfWeek;
        internal DateTime Date;
        internal string Group;
        internal string Lesson;
        internal string Teacher;
        internal string Room;

        public Change()
        {
            changesList = new List<Change>();
            var changeRows = ParserEngine.ParseChanges();
            if (changeRows.Count > 0)
            {
                foreach (var changesRow in changeRows)
                {
                    Date = DateTime.ParseExact(changesRow[1], "DD.MM.YYYY", CultureInfo.InvariantCulture);
                    DayOfWeek = Date.DayOfWeek;
                    Group = changesRow[2];
                    Lesson = changesRow[3];
                    Teacher = changesRow[4];
                    Room = changesRow[5];
                    changesList.Add(this);
                }
            }
        }

        internal List<Change> GetChangeList()
        {
            if (changesList.Count > 0)
            {
                return changesList;
            }

            return null;
        }
    }
}