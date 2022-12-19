using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityCompetition.Core.Contracts;
using UniversityCompetition.Models;
using UniversityCompetition.Models.Contracts;
using UniversityCompetition.Repositories;
using UniversityCompetition.Utilities.Messages;

namespace UniversityCompetition.Core
{
    public class Controller : IController
    {
        public Controller()
        {
            subjects = new SubjectRepository();
            students = new StudentRepository();
            universities = new UniversityRepository();
        }
        private SubjectRepository subjects;
        private StudentRepository students;
        private UniversityRepository universities;
        public string AddStudent(string firstName, string lastName)
        {
            string name = firstName + " " + lastName;
            if (students.FindByName(name) != null)
            {
                return string.Format(OutputMessages.AlreadyAddedStudent, firstName, lastName);
            }
            IStudent student = new Student(students.Models.Count + 1, firstName, lastName);
            students.AddModel(student);
            return string.Format(OutputMessages.StudentAddedSuccessfully, firstName, lastName, students.GetType().Name);
        }

        public string AddSubject(string subjectName, string subjectType)
        {
            ISubject subject = null;
            if (subjectType != "TechnicalSubject" && subjectType != "HumanitySubject" && subjectType != "EconomicalSubject")
            {
                return String.Format(OutputMessages.SubjectTypeNotSupported, subjectType);
            }
            if (subjects.FindByName(subjectName) != null)
            {
                return String.Format(OutputMessages.AlreadyAddedSubject, subjectName);
            }
            if (subjectType == "TechnicalSubject")
            {
                subject = new TechnicalSubject(subjects.Models.Count + 1, subjectName);
            }
            if (subjectType == "HumanitySubject")
            {
                subject = new HumanitySubject(subjects.Models.Count + 1, subjectName);
            }
            if (subjectType == "EconomicalSubject")
            {
                subject = new EconomicalSubject(subjects.Models.Count + 1, subjectName);
            }
            subjects.AddModel(subject);
            return String.Format(OutputMessages.SubjectAddedSuccessfully,subjectType ,subjectName, subjects.GetType().Name);
        }

        public string AddUniversity(string universityName, string category, int capacity, List<string> requiredSubjects)
        {
            if (universities.FindByName(universityName) != null)
            {
                return String.Format(OutputMessages.AlreadyAddedUniversity, universityName);
            }
            List<int> requiredsubjects = new List<int>();
            foreach (var subject in subjects.Models)
            {
                if (requiredSubjects.Contains(subject.Name))
                {
                    requiredsubjects.Add(subject.Id);
                }
            }
            University university = new University(universities.Models.Count + 1, universityName, category, capacity, requiredsubjects);
            universities.AddModel(university);
            return String.Format(OutputMessages.UniversityAddedSuccessfully, universityName, universities.GetType().Name);
        }

        public string ApplyToUniversity(string studentName, string universityName)
        {
            string[] name = studentName.Split(' ');
            string firstName = name[0];
            string lastName = name[1];
            if (students.FindByName(studentName) == null)
            {
                return string.Format(OutputMessages.StudentNotRegitered, firstName, lastName);
            }
            if (universities.FindByName(universityName) == null)
            {
                return String.Format(OutputMessages.UniversityNotRegitered, universityName);
            }
            foreach (var exam in universities.FindByName(universityName).RequiredSubjects)
            {
                if (!students.FindByName(studentName).CoveredExams.Any(x => x == exam))
                {
                    return String.Format(OutputMessages.StudentHasToCoverExams, studentName, universityName);
                }
            }
            IStudent student = students.FindByName(studentName);
            if (student.University != null)
            {
                if (student.University.Name == universityName)
                {
                    return String.Format(OutputMessages.StudentAlreadyJoined, firstName, lastName, universityName);
                }
            }
            students.FindByName(studentName).JoinUniversity(universities.FindByName(universityName));
            return String.Format(OutputMessages.StudentSuccessfullyJoined, firstName, lastName, universityName);
        }

        public string TakeExam(int studentId, int subjectId)
        {
            if (students.FindById(studentId) == null)
            {
                return String.Format(OutputMessages.InvalidStudentId);
            }
            if (subjects.FindById(subjectId) == null)
            {
                return String.Format(OutputMessages.InvalidSubjectId);
            }
            if (students.FindById(studentId).CoveredExams.Contains(subjectId))
            {
                return String.Format(OutputMessages.StudentAlreadyCoveredThatExam, students.FindById(studentId).FirstName, students.FindById(studentId).LastName, subjects.FindById(subjectId).Name);
            }
            students.FindById(studentId).CoverExam(subjects.FindById(subjectId));
            return String.Format(OutputMessages.StudentSuccessfullyCoveredExam, students.FindById(studentId).FirstName, students.FindById(studentId).LastName, subjects.FindById(subjectId).Name);
        }

        public string UniversityReport(int universityId)
        {
            IUniversity university = universities.FindById(universityId);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"*** {university.Name} ***");
            sb.AppendLine($"Profile: {university.Category}");
            int studentsEntered = 0;
            foreach (var student in students.Models.Where(x=>x.University != null))
            {
                if (student.University.Id == universityId)
                {
                    studentsEntered++;
                }
            }
            sb.AppendLine($"Students admitted: {studentsEntered}");
            sb.AppendLine($"University vacancy: {university.Capacity - studentsEntered}");
            return sb.ToString().TrimEnd();
        }
    }
}
