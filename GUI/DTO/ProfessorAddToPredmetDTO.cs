using CLI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.DTO
{
    public class ProfessorAddToPredmetDTO
    {
        public int id { get; set; }
        private string naziv;
        public string Naziv
        {
            get
            {
                return naziv;
            }
            set
            {
                if (value != naziv)
                {
                    naziv = value;
                    OnPropertyChanged("Naziv");
                }
            }
        }


        public Profesor ToProfessor()
        {
            return null;//new Predmet(id,(int)sifra, naziv, semestar, godinaStudija,idProf,(int)espb);
        }

        public ProfessorAddToPredmetDTO()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ProfessorAddToPredmetDTO(Profesor p)
        {
            id = p.id;
            naziv = p.ime +" "+ p.prezime;
        }

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
