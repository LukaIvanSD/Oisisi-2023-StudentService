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
    public class ProfessorController
    {
        private  StudentskaSluzbaDAO ssluzba;
        public ProfessorController() { 
        ssluzba=new StudentskaSluzbaDAO();
        }
        public List<Profesor> GetAllProfessors()
        {
            return ssluzba.GetAllProfessors();
        }

        public void Add(Profesor profesor)
        {
            ssluzba.AddProfesor(profesor);
        }

        public void Delete(int id)
        {
            ssluzba.RemoveProfesor(id);
        }

        public void Subscribe(IObserver observer)
        {
            ssluzba.ProfesoriSubject.Subscribe(observer);
        }
        public void Update(Profesor profesor)
        {
            ssluzba.UpdateProfessor(profesor);
        }
    }
}
