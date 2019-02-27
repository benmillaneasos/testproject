using DataModels;
using ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create a few Data Model Students
            StudentModel studentModel1 = new StudentModel { Id = 1, FirstName = "test", LastName = "student" };
            StudentModel studentModel2 = new StudentModel { Id = 2, FirstName = "test", LastName = "student2" };
            StudentModel studentModel3 = new StudentModel { Id = 3, FirstName = "test", LastName = "student3" };

            //Add to DB
            Database.CreateStudent(studentModel1);
            Database.CreateStudent(studentModel2);
            Database.CreateStudent(studentModel3);

            //Get Object Model Students
            Student student1 = new Student(1);
            Student student2 = new Student(2);
            Student student3 = new Student(3);

            //Create some dance lessons
            LessonModel dance1 = new LessonModel { LessonType = (int)LessonType.Dance, id = 1, DateOfLesson = new DateTime(2019, 03, 04), LengthInHours = 1, RegisterTaken = false };
            LessonModel dance2 = new LessonModel { LessonType = (int)LessonType.Dance, id = 2, DateOfLesson = new DateTime(2019, 03, 11), LengthInHours = 1, RegisterTaken = false };
            LessonModel dance3 = new LessonModel { LessonType = (int)LessonType.Dance, id = 3, DateOfLesson = new DateTime(2019, 03, 18), LengthInHours = 1, RegisterTaken = false };
            //Create some acting lessons
            LessonModel acting1 = new LessonModel { LessonType = (int)LessonType.Acting, id = 4, DateOfLesson = new DateTime(2019, 03, 04), LengthInHours = 1, RegisterTaken = false };
            LessonModel acting2 = new LessonModel { LessonType = (int)LessonType.Acting, id = 5, DateOfLesson = new DateTime(2019, 03, 11), LengthInHours = 1, RegisterTaken = false };
            LessonModel acting3 = new LessonModel { LessonType = (int)LessonType.Acting, id = 6, DateOfLesson = new DateTime(2019, 04, 18), LengthInHours = 1, RegisterTaken = false };
            //Create some singing lessons
            LessonModel singing1 = new LessonModel { LessonType = (int)LessonType.Singing, id = 7, DateOfLesson = new DateTime(2019, 04, 18), LengthInHours = 1, RegisterTaken = false };
            LessonModel singing2 = new LessonModel { LessonType = (int)LessonType.Singing, id = 8, DateOfLesson = new DateTime(2019, 04, 25), LengthInHours = 1, RegisterTaken = false };

            //Add Lessons to Database
            Database.CreateLesson(dance1);
            Database.CreateLesson(dance2);
            Database.CreateLesson(dance3);
            Database.CreateLesson(acting1);
            Database.CreateLesson(acting2);
            Database.CreateLesson(acting3);
            Database.CreateLesson(singing1);
            Database.CreateLesson(singing2);

            //Sign up students to various lessons
            SignUpHelper signupHelper = new SignUpHelper();
            signupHelper.SignUpToLesson(student1.id, LessonType.Dance);
            signupHelper.SignUpToLesson(student1.id, LessonType.Acting);
            signupHelper.SignUpToLesson(student2.id, LessonType.Dance);
            signupHelper.SignUpToLesson(student3.id, LessonType.Acting);
            signupHelper.SignUpToLesson(student3.id, LessonType.Singing);

            //Take First Lesson (id == 1, which is a dance lesson)
            Lesson lesson1 = new Lesson(1);
            //all who have signed up are attending this lesson. So no absence will be recorded
            List<int> lesson1StudentsAttending = new List<int> { 1, 2 };
            lesson1.TakeRegister(lesson1StudentsAttending);
            //try to take register again and you can't
            lesson1.TakeRegister(lesson1StudentsAttending);

            //Take First Lesson (id == 2, which is a dance lesson)
            Lesson lesson2 = new Lesson(2);
            //one student is missing now
            List<int> lesson2StudentsAttending = new List<int> { 1 };
            lesson2.TakeRegister(lesson2StudentsAttending);

            //Get Absence
            AbsenceHelper absencehelper = new AbsenceHelper();
            var student2lesson2absence = absencehelper.GetAbsenceIdForStudentAndLesson(2, 2);

            Console.ReadLine();


        }
    }
}
