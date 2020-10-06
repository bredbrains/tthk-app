using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using tthk_app.ParsingService;

namespace tthk_app.Models
{
    public class Change
    {
        public DayOfWeek DayOfWeek { get; set; }
        public string Date { get; set; }
        public string Group { get; set; }
        public string Lesson { get; set; }
        public string Teacher { get; set; }
        public string Room { get; set; }
    }
}