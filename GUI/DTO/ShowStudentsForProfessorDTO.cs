using CLI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.DTO
{
    public class ShowStudentsForProfessorDTO
    {
        public int id { get; set; }
        public string ime { get; set; }

        public ShowStudentsForProfessorDTO()
        {

        }

        public ShowStudentsForProfessorDTO(Student s)
        {
            id = s.id;
            ime = s.ime + " " + s.prezime + " " + s.brojIndeksa;
        }
    }
}
