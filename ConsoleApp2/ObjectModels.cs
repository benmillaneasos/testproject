using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectModels
{
    public class Student
    {
        StudentModel studentModel;
        public int id { get { return studentModel.Id; } }
        public string FirstName { get { return studentModel.FirstName; } }
        public string LastName { get { return studentModel.LastName; } }
        public Student (int id)
        {
            this.studentModel = Database.GetStudent(id);
        }

        public List<int> GetLessonsSignedUpTo()
        {
            return Database.LessonSignUps.Where(c => c.Value.Contains(this.id)).Select(x => x.Key).ToList();
        }

        public void HasApologisedForAbsence(int absenceId)
        {
            Absence absence = new Absence(absenceId);
            absence.UpdateHasApologised();
        }

        public void ReasonForAbsence(int absenceId, string reason)
        {
            Absence absence = new Absence(absenceId);
            absence.UpdateAbsenceReason(reason);
        }
    }

    public class Absence
    {
        AbsenceModel AbsenceModel;
        public int Id { get { return AbsenceModel.Id; } }
        public string Reason { get { return AbsenceModel.Reason; } }
        public bool HasApologised { get { return AbsenceModel.HasApologised; } }
        public int StudentId { get { return AbsenceModel.StudentId; } }
        public int LessonId { get { return AbsenceModel.LessonId; } }

        public Absence(int absenceId)
        {
            this.AbsenceModel = Database.GetAbsence(absenceId);
        }

        public void UpdateAbsenceReason(string reason)
        {
            this.AbsenceModel.Reason = reason;
            Database.UpdateAbsence(this.AbsenceModel);
        }

        public void UpdateHasApologised()
        {
            this.AbsenceModel.HasApologised = true;
            Database.UpdateAbsence(this.AbsenceModel);
        }
    }

    public class AbsenceHelper
    {
        public AbsenceModel GetAbsenceIdForStudentAndLesson(int studentId, int lessonId)
        {
            return Database.Absences.Where(x => x.StudentId == studentId && x.LessonId == lessonId).Single();
        }
    }

    public class SignUpHelper
    {
        public void SignUpToLesson(int StudentID, LessonType LessonID)
        {
            Database.SignUpToLesson(StudentID, (int)LessonID);
        }
    }

    public class Lesson
    {
        LessonModel lessonModel;

        public Lesson(int lessonId)
        {
            this.lessonModel = Database.GetLessson(lessonId);
        }

        public void TakeRegister(List<int> StudentsInLesson)
        {
            if (lessonModel.RegisterTaken)
            {
                Console.WriteLine("Register already taken for this lesson");
                return;
            }
            else
            {
                //Get list of students not attending lesson who are signed up
                List<int> StudentsSignedUp = Database.LessonSignUps[lessonModel.LessonType];
                List<int> AbsentStudents = StudentsSignedUp.Except(StudentsInLesson).ToList();

                foreach(int s in AbsentStudents)
                {
                    Database.NumberOfAbsences += 1;
                    AbsenceModel absence = new AbsenceModel();
                    absence.Id = Database.NumberOfAbsences;
                    absence.LessonId = lessonModel.id;
                    absence.StudentId = s; 
                    absence.HasApologised = false;
                    Database.CreateAbsence(absence);
                    Console.WriteLine("Student {0} is absent today", s);
                }

                lessonModel.RegisterTaken = true;
                LessonModel lesson = Database.GetLessson(lessonModel.id);
                lesson.RegisterTaken = lessonModel.RegisterTaken;
                Database.UpdateLesson(lesson);
            }
        }
    }

    public static class Database
    {
        //ID FIELDS
        public static int NumberOfAbsences = 0;

        public static Dictionary<int, List<int>> LessonSignUps = new Dictionary<int, List<int>>(); //LessonType,StudentId
        public static List<StudentModel> Students = new List<StudentModel>();
        public static List<LessonModel> Lessons = new List<LessonModel>();
        public static List<AbsenceModel> Absences = new List<AbsenceModel>();

        //STUDENTS
        public static void CreateStudent(StudentModel student)
        {
            Students.Add(student);
        }
        public static StudentModel GetStudent(int id)
        {
            return Students.Where(x => x.Id == id).Single();
        }

        //ABSENCES
        public static void CreateAbsence(AbsenceModel absence)
        {
            Absences.Add(absence);
        }
        public static List<AbsenceModel> GetAllStudentAbsences(int studentId)
        {
            return Absences.Where(x => x.StudentId == studentId).ToList();
        }
        public static AbsenceModel GetAbsence(int absenceId)
        {
            return Absences.Where(x => x.Id == absenceId).Single();
        }
        public static void UpdateAbsence(AbsenceModel absence)
        {
            var index = Absences.FindIndex(x => x.Id == absence.Id);
            Absences[index] = absence;
        }

        //LESSONS
        public static void CreateLesson(LessonModel lesson)
        {
            //TODO Logic to add all students in signup list to this lesson
            Lessons.Add(lesson);
        }
        public static void SignUpToLesson(int StudentId, int LessonType)
        {
            List<int> existingValues;
            if (LessonSignUps.ContainsKey(LessonType))
            {
                existingValues = LessonSignUps[LessonType];
                if (existingValues.Contains(StudentId))
                {
                    Console.WriteLine("Student already signed up to lesson");
                    return;
                }
                else
                {
                    //Update existing entry with new student ID
                    existingValues.Add(StudentId);
                    LessonSignUps[LessonType] = existingValues;
                }
            }
            else
            {
                //Add new entry to dictionary
                existingValues = new List<int>();
                existingValues.Add(StudentId);
                LessonSignUps.Add(LessonType, existingValues);
            }
        }
        public static LessonModel GetLessson(int lessonId)
        {
            return Lessons.Where(x => x.id == lessonId).Single();
        }
        public static void UpdateLesson(LessonModel lesson)
        {
            var index = Lessons.FindIndex(x => x.id == lesson.id);
            Lessons[index] = lesson;
        }
    }
}
