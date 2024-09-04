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
    public class StudentPredmetController
    {
        private StudentskaSluzbaDAO ssluzba;
        public StudentPredmetController()
        {
            ssluzba = new StudentskaSluzbaDAO();
        }
        public List<StudentPredmet> GetStudentPredmets()
        {
            return ssluzba.GetAllStudentPredmet();
        }

        public void Add(StudentPredmet studentPredmet)
        {
            ssluzba.AddStudentPredmet(studentPredmet);
        }

       /* public void Delete(int id) NEMAMO IMPLEMENTACIJU ZA DELETE DA LI JE TREBA DODATI??
        {
            ssluzba.Remove(id);
        }*/

        public void Subscribe(IObserver observer)
        {
            ssluzba.StudentPredmetSubject.Subscribe(observer);
        }
       /* public void Update(Katedra katedra) NEMAMO IMPLEMENTACIJU ZA Update
        {
            ssluzba.UpdateStu(katedra);
        }*/
    }
}
