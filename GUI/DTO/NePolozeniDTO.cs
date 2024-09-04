using CLI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.DTO
{
    public class NePolozeniDTO
    {
        public int id { get; set; }

        private int sifra;
        public int Sifra
        {
            get
            {
                return sifra;
            }
            set
            {
                if (value != sifra)
                {
                    sifra = value;
                    OnPropertyChanged("Sifra");
                }
            }
        }
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
        private int espb;
        public int Espb
        {
            get
            {
                return espb;
            }
            set
            {
                if (value != espb)
                {
                    espb = value;
                    OnPropertyChanged("Espb");
                }
            }
        }
        private int godinaStudija;
        public int GodinaStudija
        {
            get
            {
                return godinaStudija;
            }
            set
            {
                if (value != godinaStudija)
                {

                    godinaStudija = value;
                    OnPropertyChanged("GodinaStudija");
                }
            }
        }
        private string semestar;
        public string Semestar
        {
            get
            {
                return semestar;
            }
            set
            {
                if (value != semestar)
                {
                    semestar = value;
                    OnPropertyChanged("Semestar");
                }
            }
        }
        public Predmet ToPredmet()
        {
            return null; //new Predmet(id, (int)sifra, naziv, (int)espb);
        }

        public NePolozeniDTO()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public NePolozeniDTO(Predmet p)
        {
            naziv = p.naziv;
            sifra = p.sifra;
            espb = p.espb;
            id = p.id;
            semestar=p.semestar.ToString();
            godinaStudija = p.godinaStudija;
           

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
