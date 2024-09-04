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
    public class StudentController
    {

        private StudentskaSluzbaDAO ssluzba;

        public StudentController()
        {
            ssluzba = new StudentskaSluzbaDAO();
        }

        public List<Student> GetAllStudents()
        {
            return ssluzba.GetAllStudents();
        }

        public void Add(Student student)
        {
            ssluzba.AddStudent(student);
        }

        public void Delete(int id)
        {
            ssluzba.RemoveStudent(id);
        }

        public void Subscribe(IObserver observer)
        {
            ssluzba.StudentiSubject.Subscribe(observer);
        }
        public void Update(Student student) 
        {
            ssluzba.UpdateStudent(student);
        }
    }
    
}
