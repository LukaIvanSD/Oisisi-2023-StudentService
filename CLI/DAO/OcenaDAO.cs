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
    public class OcenaDAO
    {
        public List<Ocena> _ocene { get; set; }

        public Subject OceneSubject;

        private readonly Storage<Ocena> _ocenaStorage;


        public OcenaDAO()
        {

            _ocenaStorage = new Storage<Ocena>("ocene.txt");

            OceneSubject = new Subject();

            _ocene = _ocenaStorage.Load();

            _ocene.ForEach(o => { o.id = _ocene.IndexOf(o); });

        }

        private int GenerateOcenaId()
        {
            if (_ocene.Count == 0) return 0;
            return _ocene.Max(o => o.id) + 1;
        }

        public void izracunajProsek(int studentId)
        {
            double zbirOcena = 0;
            int brojac = 0;

            foreach (Ocena o in _ocene)
            {
                if (o.idStudentPolozio == studentId)
                {
                    zbirOcena += o.ocena;
                    brojac++;
                }
            }
        }


        }
}
