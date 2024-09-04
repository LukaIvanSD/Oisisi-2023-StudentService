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
    public class KatedraDAO
    {
        public List<Katedra> _katedre { get; set; }

        public Subject KatedreSubject;

        private readonly Storage<Katedra> _katedraStorage;

        public KatedraDAO(ProfesorDAO profesorDAO)
        {
            _katedraStorage = new Storage<Katedra>("Katedre.txt");
            KatedreSubject = new Subject();
            _katedre = _katedraStorage.Load();

            if (_katedre.Count != 0)
            {
                _katedre.ForEach(k => { k.profesori = new List<Profesor>(); });
            }

            foreach (Profesor prof in profesorDAO._profesori)
            {

                _katedre.ForEach(k =>
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
