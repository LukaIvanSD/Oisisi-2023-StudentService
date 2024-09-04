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
    public class PolozeniDTO:INotifyPropertyChanged,IDataErrorInfo
    {
        StudentskaSluzbaDAO ssluzba;
        public int idOcene { get; set; }

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
        private int ocena=6;
        public int Ocena
        {
            get
            {
                return ocena;
            }
            set
            {
                if (value != ocena)
                {
                    ocena = value;
                    Debug.WriteLine(ocena);
                    OnPropertyChanged("Ocena");
                }
            }
        }
        private string datum;
        public string Datum
        {
            get
            {
                return datum;
            }
            set
            {
                if (value != datum)
                {
                    datum = value;
                    OnPropertyChanged("Datum");
                }
            }
        }
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Espb")
                {
                    if (Espb < 0)
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
                    if (sifra < 0)
                    {
                        return "Code is less than 0";
                    }
                }
                else if (columnName == "Ocena")
                {
                    if (Ocena<5)
                    {
                        return "Mark cannot be less than 5";
                    }
                }
                else if (columnName == "Datum")
                {
                    if (string.IsNullOrEmpty(datum))
                    {
                        return "Date is not valid";
                    }
                    if (!ssluzba.DatumRegex(datum))
                        return "Format not valid, try again.";
                }
                return null;
            }
        }
        private readonly string[] _validatedProperties = {"Espb", "Naziv", "Sifra","Ocena","Datum" };

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

        public PolozeniDTO(StudentskaSluzbaDAO ssluzba)
        {
            this.ssluzba = ssluzba;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public PolozeniDTO(Predmet p,int ocena,string datum,int idOcena)
        {
            naziv = p.naziv;
            sifra = p.sifra;
            espb = p.espb;
            idOcene = idOcena;
            this.ocena= ocena;
            this.datum = datum;
            
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
