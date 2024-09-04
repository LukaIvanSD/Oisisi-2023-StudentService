using CLI.DAO;
using CLI.Model;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Controller
{
    public class OceneController
    {
        private StudentskaSluzbaDAO ssluzba;
        public OceneController()
        {
            ssluzba = new StudentskaSluzbaDAO();
        }
        public List<Ocena> GetAllOcene()
        {
            return ssluzba.GetAllOcene();
        }

        public void Add(Ocena ocena)
        {
            ssluzba.AddOcena(ocena);
        }

        public void Delete(int id)
        {
            ssluzba.RemoveOcena(id);
        }

        public void Subscribe(IObserver observer)
        {
            ssluzba.OceneSubject.Subscribe(observer);
        }
        public void Update(Ocena ocena)
        {
            ssluzba.UpdateOcena(ocena);
        }
    }
}
