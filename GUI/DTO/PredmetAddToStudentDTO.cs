using CLI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.DTO
{
    public class PredmetAddToStudentDTO
    {
        public int id { get; set; } 
        private string ime;
        public string Ime
        {
            get
            {
                return ime;
            }
            set
            {
                if (value != ime)
                {
                    ime = value;
                    OnPropertyChanged("Ime");
                }
            }
        }


        public Predmet ToPredmet()
        {
            return null;//new Predmet(id,(int)sifra, naziv, semestar, godinaStudija,idProf,(int)espb);
        }

        public PredmetAddToStudentDTO()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public PredmetAddToStudentDTO(Predmet p)
        {
            id = p.id;
            ime = p.sifra + " - " + p.naziv;
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
