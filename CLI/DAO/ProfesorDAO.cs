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
    public class ProfesorDAO
    {
        public List<Profesor> _profesori { get; set; }
        public Subject ProfesoriSubject;
        private readonly Storage<Profesor> _profesorStorage;

        public ProfesorDAO(KatedraDAO katedraDAO)
        {
            _profesorStorage = new Storage<Profesor>("profesori.txt");
            ProfesoriSubject = new Subject();
            _profesori = _profesorStorage.Load();

            if (_profesori.Count != 0)
            {
                _profesori.ForEach(p => { p.predaje = new List<Predmet>(); });
            }

            foreach (Profesor prof in _profesori)
            {

                katedraDAO._katedre.ForEach(k =>
                {
                    if (k.sefId == prof.id)
                    {
                        k.sef = prof;
                        k.profesori.Add(prof);
                    }
                    else if (k.id == prof.idKatedre)
                    {
                        k.profesori.Add(prof);
                    }
                });
            }
        }
    }
}
