using CLI.Model;
using CLI.Observer;
using CLI.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.DAO
{
    public class StudentDAO
    {
        public List<Student> _studenti { get; set; }

        public Subject StudentiSubject;

        private readonly Storage<Student> _studentStorage;

        public StudentDAO(List<StudentPredmet> _studentpredmet, List<Predmet> _predmeti)
        {
            _studentStorage = new Storage<Student>("studenti.txt");

            _studenti = _studentStorage.Load();

            _studenti.ForEach(s => { s.id = _studenti.IndexOf(s); });

            if (_studenti.Count != 0)
            {
                _studenti.ForEach(s => { s.nepolozeniIspiti = new List<Predmet>(); s.polozeniIspiti = new List<Predmet>(); });
            }

            foreach (Student s in _studenti)
            {
                _studentpredmet.ForEach(sp =>
                {
                    if (sp.StudentId == s.id)
                    {
                        s.nepolozeniIspiti.Add(GetPredmetById(sp.PredmetId, _predmeti));
                    }
                });
            }

        }

        public Predmet? GetPredmetById(int id, List<Predmet> _predmeti)
        {
            return _predmeti.Find(p => p.id == id);
        }

    }
}
