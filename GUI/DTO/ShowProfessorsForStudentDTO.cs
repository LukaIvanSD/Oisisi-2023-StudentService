using CLI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.DTO
{
    public class ShowProfessorsForStudentDTO
    {
        public int id { get; set; }
        public string ime { get; set; }

        public ShowProfessorsForStudentDTO()
        { 

        }

        public ShowProfessorsForStudentDTO(Profesor profesor)
        {
            id = profesor.id;
            ime = profesor.ime + " " + profesor.prezime;
        }
    }
}
