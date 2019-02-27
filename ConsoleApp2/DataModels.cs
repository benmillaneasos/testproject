using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class LessonModel
    {
        public int id { get; set; }
        public int LessonType { get; set; }
        public DateTime DateOfLesson { get; set; }
        public int LengthInHours { get; set; }
        public bool RegisterTaken { get; set; }
    }

    public class AbsenceModel
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public bool HasApologised { get; set; }
        public int StudentId { get; set; }
        public int LessonId { get; set; }
    }

    public enum LessonType
    {
        Dance = 1,
        Acting = 2,
        Singing = 3
    }
}
