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
    public class KatedraController
    {
        private StudentskaSluzbaDAO ssluzba;
        public KatedraController()
        {
            ssluzba = new StudentskaSluzbaDAO();
        }
        public List<Katedra> GetAllKatedre()
        {
            return ssluzba.GetAllKatedre();
        }

        public void Add(Katedra katedra)
        {
            ssluzba.AddKatedra(katedra);
        }

        public void Delete(int id)
        {
            ssluzba.RemoveKatedra(id);
        }

        public void Subscribe(IObserver observer)
        {
            ssluzba.KatedreSubject.Subscribe(observer);
        }
        public void Update(Katedra katedra)
        {
            ssluzba.UpdateKatedra(katedra);
        }
    }
}
