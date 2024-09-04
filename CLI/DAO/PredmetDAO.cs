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
    public class PredmetDAO
    {
        public List<Predmet> _predmeti { get; set; }

        public Subject PredmetSubject;

        private readonly Storage<Predmet> _predmetStorage;

        public PredmetDAO(List<StudentPredmet> _studentpredmet, List<Student> _studenti)
        {
            _predmetStorage = new Storage<Predmet>("predmeti.txt");

            PredmetSubject = new Subject();

            _predmeti = _predmetStorage.Load();

            _predmeti.ForEach(pr => { pr.id = _predmeti.IndexOf(pr); });

            if (_predmeti.Count != 0)
            {
                _predmeti.ForEach(p => { p.nisuPoloziliPredmet = new List<Student>(); p.poloziliPredmet = new List<Student>(); });
            }

            foreach (Predmet p in _predmeti)
            {
                _studentpredmet.ForEach(sp =>
                {
                    if (sp.PredmetId == p.id)
                    {
                        p.nisuPoloziliPredmet.Add(GetStudentById(sp.StudentId, _studenti));
                    }
                });
            }
        }

        public Student? GetStudentById(int id, List<Student> _studenti)
        {
            return _studenti.Find(s => s.id == id);
        }
    }
}
