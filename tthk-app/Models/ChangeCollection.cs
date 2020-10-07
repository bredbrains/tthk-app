using System;
using System.Collections.Generic;
using System.Globalization;
using tthk_app.ParsingService;
using Xamarin.Essentials;

namespace tthk_app.Models
{
    public class ChangeCollection
    {
        public static IEnumerable<Change> GetChangeList()
        {
            var changesList = new List<Change>();
            List<List<string>> changeRows;
            changeRows = ParserEngine.ParseChanges();
            App.Database.ClearTable();
            if (changeRows.Count > 0)
            {
                foreach (var changesRow in changeRows)
                {
                    Change change = new Change();
                    DateTime changeDate =
                        DateTime.ParseExact(changesRow[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    change.Date = changeDate.ToString("dd.MM.yyyy");
                    change.DayOfWeek = (int)changeDate.DayOfWeek;
                    change.Group = changesRow[2];
                    change.Lesson = changesRow[3].Replace("&#8211;", "-");
                    change.Teacher = changesRow[4];
                    if (changesRow.Count > 5)
                    {
                        change.Room = changesRow[5];
                    }
                    else
                    {
                        change.Room = "";
                    }

                    App.Database.SaveItemAsync(change);
                    changesList.Add(change);
                }
                IEnumerable<Change> changesEnum = changesList;
                return changesEnum;
            }
            return null;
        }
    }
}