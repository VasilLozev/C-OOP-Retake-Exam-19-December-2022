using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityCompetition.Models.Contracts;
using UniversityCompetition.Repositories.Contracts;

namespace UniversityCompetition.Repositories
{
    public class StudentRepository : IRepository<IStudent>
    {
        public StudentRepository()
        {
            models = new List<IStudent>();
        }
        private List<IStudent> models;
        public IReadOnlyCollection<IStudent> Models
        {
            get { return models.AsReadOnly(); }
        }

        public void AddModel(IStudent model)
        {
            models.Add(model);
        }

        public IStudent FindById(int id)
        {
            return models.FirstOrDefault(x => x.Id == id);
        }

        public IStudent FindByName(string name)
        {
            return models.FirstOrDefault(x => x.FirstName == name.Split(" ")[0] &&
            x.LastName == name.Split(" ")[1]);
        }
    }
}
