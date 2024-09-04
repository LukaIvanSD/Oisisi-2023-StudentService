using CLI.DAO;
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
    public class PredmetDTO:INotifyPropertyChanged,IDataErrorInfo
    {//TREBA IZMENITI LOGIKU ZA COMBOBOX!!!!!
        private StudentskaSluzbaDAO ssluzba;
        public int id { get; set; }

        private int? sifra;
        public int? Sifra
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
        private int? espb;
        public int? Espb
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
        private int godinaStudija=1;
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
                    Debug.WriteLine(godinaStudija);
                    OnPropertyChanged("GodinaStudija");
                }
            }
        }
        private int godinaStudijaString=0;
        public int GodinaStudijaString
        {
            get
            {
                return godinaStudijaString;
            }
            set
            {
                if (value != godinaStudijaString)
                {

                    godinaStudijaString = value;
                    godinaStudija = godinaStudijaString+1;
                    OnPropertyChanged("GodinaStudijaString");
                }
            }
        }
        private Semestar semestar=Semestar.Letnji;
        public Semestar Semestar
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
        private int semestarCombo=0;
        public int SemestarCombo
        {
            get
            {
                return semestarCombo;
            }
            set
            {
                if (value != semestarCombo)
                {
                    semestarCombo = value;
                    semestar = (Semestar)semestarCombo;
                    OnPropertyChanged("SemestarCombo");
                }
            }
        }
        private int idProf;
        public int IdProf
        {
            get
            {
                return idProf;
            }
            set
            {
                if (value != idProf)
                {
                    idProf = value;
                    OnPropertyChanged("IdProf");
                }
            }
        }
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "GodinaStudija")
                {
                    if(godinaStudija>4 || godinaStudija<1)
                        return "Year of study not valid";


                }
                else if (columnName == "IdProf")
                {
                    if (idProf <-1)
                        return "Professor ID cannot be less than -1";
                   
                    if (ssluzba.GetAllProfessors().FindIndex(p => p.id == idProf) == -1 && idProf!=-1 )
                    return "Not valid";
                }
                else if (columnName == "Espb")
                {
                    if (!espb.HasValue)
                        return "Not valid";
                    if (espb<0)
                    {
                        return "Espb cannot be less than 0";
                    } 
                }
                else if (columnName == "Naziv")
                {
                    if (string.IsNullOrEmpty(Naziv))
                    {
                        return "Not valid";
                    }
                }
                else if (columnName == "Sifra")
                {
                    if (!sifra.HasValue)
                        return "Not valid";
                    if (sifra<0)
                    {
                        return "Code is less than 0";
                    }
                }
                return null;
            }
        }
        private readonly string[] _validatedProperties = { "GodinaStudija", "IdProf","Espb","Naziv","Sifra"};

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
        public Predmet ToPredmet()
        {
            return new Predmet(id, (int)sifra, naziv,semestar, godinaStudija, (int)idProf, (int)espb);
        }

        public PredmetDTO(StudentskaSluzbaDAO ssluzba)
        {
            this.ssluzba = ssluzba;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public PredmetDTO(Predmet p,StudentskaSluzbaDAO ssluzba=null)
        {
            this.ssluzba = ssluzba;
            naziv = p.naziv;
            sifra = p.sifra;
            id = p.id;
            godinaStudija = p.godinaStudija;
            semestar = p.semestar;
            espb = p.espb;
            idProf = p.professorId;
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
