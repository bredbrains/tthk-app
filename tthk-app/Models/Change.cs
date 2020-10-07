using System;
using SQLite;

namespace tthk_app.Models
{
    public class Change
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int DayOfWeek { get; set; }
        public string Date { get; set; }
        public string Group { get; set; }
        public string Lesson { get; set; }
        public string Teacher { get; set; }
        public string Room { get; set; }
    }
}