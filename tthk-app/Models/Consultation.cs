using System;
using System.Collections.Generic;

namespace tthk_app.Models
{
    class Consultation
    {
        public string Teacher { get; set; }
        public string Room { get; set; }
        public List<Dictionary<DayOfWeek, string>> Time { get; set; }
        public Department TeacherDepartment { get; set; }

        public Consultation(string teacher, string room, List<Dictionary<DayOfWeek, string>> time, Department department)
        {
            Teacher = teacher;
            Room = room;
            Time = time;
            TeacherDepartment = department;
        }
    }
}
