using System;
using System.Collections.Generic;
using System.Text;
using UniversityCompetition.Models.Contracts;
using UniversityCompetition.Utilities.Messages;

namespace UniversityCompetition.Models
{
    public abstract class Subject : ISubject
    {
        public Subject(int subjectId, string subjectName, double subjectRate)
        {
            Id = subjectId;
            Name = subjectName;
            Rate = subjectRate;
        }
        private int id;

        public int Id
        {
            get { return id; }
            private set { id = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            private set {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(string.Format(ExceptionMessages.NameNullOrWhitespace));
                }
                name = value; }
        }

        private double rate;

        public double Rate
        {
            get { return rate; }
            private set { rate = value; }
        }
    }
}
