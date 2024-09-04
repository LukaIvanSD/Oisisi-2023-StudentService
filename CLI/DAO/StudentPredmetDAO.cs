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
    public class StudentPredmetDAO
    {
        public List<StudentPredmet> _studentpredmet { get; set; }

        public Subject StudentPredmetSubject;

        private readonly Storage<StudentPredmet> _studentpredmetStorage;

        public StudentPredmetDAO(List<Student> _studenti, List<Predmet> _predmeti)
        {
            _studentpredmetStorage = new Storage<StudentPredmet>("StudentPredmet.txt");
            StudentPredmetSubject = new Subject();
            _studentpredmet = _studentpredmetStorage.Load();
            foreach (Student s in _studenti)
            {
                _studentpredmet.ForEach(sp =>
                {
                    if (sp.StudentId == s.id)
                    {
                        s.nepolozeniIspiti.Add(GetPredmetById(sp.PredmetId,_predmeti));
                    }
                });
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

        public Predmet? GetPredmetById(int id, List<Predmet> _predmeti)
        {
            return _predmeti.Find(p => p.id == id);
        }

        public Student? GetStudentById(int id, List<Student> _studenti)
        {
            return _studenti.Find(s => s.id == id);
        }

    }
}
