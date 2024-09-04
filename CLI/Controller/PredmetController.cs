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
    public class PredmetController
    {
        private StudentskaSluzbaDAO ssluzba;
        public PredmetController()
        {
            ssluzba = new StudentskaSluzbaDAO();
        }
        public List<Predmet> GetAllPredmets()
        {
            return ssluzba.GetAllPredmeti();
        }

        public void Add(Predmet predmet)
        {
            ssluzba.AddPredmet(predmet);
        }

        public void Delete(int id)
        {
            ssluzba.RemovePredmet(id);
        }

        public void Subscribe(IObserver observer)
        {
            ssluzba.PredmetSubject.Subscribe(observer);
        }
        public void Update(Predmet predmet)
        {
            ssluzba.UpdatePredmet(predmet);
        }
    }
}
