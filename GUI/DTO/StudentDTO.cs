using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CLI.DAO;
using CLI.Model;

namespace GUI.DTO
{
    public class StudentDTO : INotifyPropertyChanged,IDataErrorInfo
    {//TREBA IZMENITI ZBOG COMBOBOXA
        private Datum datum;
        private Adresa adresa;
        private Indeks indeks;
        private StudentskaSluzbaDAO ssluzba;
        //private Status status;
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
        private string prezime;
        public string Prezime
        {
            get
            {
                return prezime;
            }
            set
            {
                if (value != prezime)
                {
                    prezime = value;
                    OnPropertyChanged("Prezime");
                }
            }
        }

        private int trenutnaGodinaStudija=1;
        public int TrenutnaGodinaStudija
        {
            get
            {
                return trenutnaGodinaStudija;
            }
            set
            {
                if (value != trenutnaGodinaStudija)
                {
                    trenutnaGodinaStudija = value;
                    OnPropertyChanged("TrenutnaGodinaStudija");
                }
            }
        }
        private int trenutnaGodinaStudijaCombo = 0;
        public int TrenutnaGodinaStudijaCombo
        {
            get
            {
                return trenutnaGodinaStudijaCombo;
            }
            set
            {
                if (value != trenutnaGodinaStudijaCombo)
                {
                    trenutnaGodinaStudijaCombo = value;
                    trenutnaGodinaStudija = value + 1;
                    OnPropertyChanged("TrenutnaGodinaStudija");
                }
            }
        }

        private double prosecnaOcena;
        public double ProsecnaOcena
        {
            get
            {
                return prosecnaOcena;
            }
            set
            {
                if (value != prosecnaOcena)
                {
                    prosecnaOcena = value;
                    OnPropertyChanged("ProsecnaOcena");
                }
            }
        }
         private Status status=Status.B;
         public Status Status
         {
             get
             {
                 return status;
             }
             set
             {
                 if (value != status)
                 {
                     Debug.WriteLine(value);
                     status = value;
                     OnPropertyChanged("Status");
                 }
             }
         }
        private int statusCombo = 0;
        public int StatusCombo
        {
            get
            {
                return statusCombo;
            }
            set
            {
                if (value != statusCombo)
                {
                    statusCombo = value;
                    status = (Status)statusCombo;
                    OnPropertyChanged("Status");
                }
            }
        }
        private string datumRodjenja;
        public string Datum {
            get
            {
                return datumRodjenja;
            }
            set
            {
                if (value != datumRodjenja)
                {
                    datumRodjenja = value;
                    OnPropertyChanged("Datum");
                }
            }
        }
        private string godinaUpisa;
        public string GodinaUpisa
        {
            get
            {
                return godinaUpisa;
            }
            set
            {
                if (value != godinaUpisa)
                {
                    godinaUpisa = value;
                    OnPropertyChanged("GodinaUpisa");
                }
            }
        }
        private string adresaStanovanja;
        public string Adresa
        {
            get
            {
                return adresaStanovanja;
            }
            set
            {
                if (value != adresaStanovanja)
                {
                    adresaStanovanja = value;
                    OnPropertyChanged("Adresa");
                }
            }
        }
        private string kontaktTelefon;
        public string KontaktTelefon
        {
            get
            {
                return kontaktTelefon;
            }
            set
            {
                if (value != kontaktTelefon)
                {
                    kontaktTelefon = value;
                    OnPropertyChanged("KontaktTelefon");
                }
            }
        }
        private string emailAdresa;
        public string EmailAdresa
        {
            get
            {
                return emailAdresa;
            }
            set
            {
                if (value != emailAdresa)
                {
                    emailAdresa = value;
                    OnPropertyChanged("EmailAdresa");
                }
            }
        }
        private string brojIndeksa;
        public string Indeks
        {
            get
            {
                return brojIndeksa;
            }
            set
            {
                if (value != brojIndeksa)
                {
                    brojIndeksa = value;
                    OnPropertyChanged("Indeks");
                }
            }
        }
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "TrenutnaGodinaStudija")
                {
                    if (trenutnaGodinaStudija<1 || trenutnaGodinaStudija>4)
                        return "Year of study must be between 1 and 4";
                }
                if (columnName == "Datum")
                {
                    if (string.IsNullOrEmpty(datumRodjenja))
                        return "Date is required";
                    if (!ssluzba.DatumRegex(datumRodjenja))
                        return "Format not good.Try again.";
                }
                else if (columnName == "Adresa")
                {
                    if (string.IsNullOrEmpty(adresaStanovanja))
                        return "Adress is required";
                    if (!ssluzba.AdresaRegex(adresaStanovanja))
                        return "Format not good. Try again.";
                }
                else if (columnName == "Indeks")
                {
                    if (string.IsNullOrEmpty(brojIndeksa))
                        return "Index is required";
                    if (!ssluzba.IndexRegex(brojIndeksa))
                        return "Format not good. Try again.";
                }
                else if (columnName == "Ime")
                {
                    if (string.IsNullOrEmpty(Ime))
                        return "Name is required";
                }
                else if (columnName == "KontaktTelefon")
                {
                    if (string.IsNullOrEmpty(KontaktTelefon))
                        return "Phone number is required";
                }
                else if (columnName == "EmailAdresa")
                {
                    if (string.IsNullOrEmpty(EmailAdresa))
                        return "E-mail adress is required";
                }
                else if (columnName == "Prezime")
                {
                    if (string.IsNullOrEmpty(Prezime))
                        return "Surname is required";
                }
                else if (columnName == "GodinaUpisa")
                {
                    if (string.IsNullOrEmpty(godinaUpisa))
                        return "Year of enrollment is required";
                    else if(!string.IsNullOrEmpty(brojIndeksa))
                    {
                        if (brojIndeksa.Split('/')[1] != godinaUpisa)
                            return "Doesnt match the index";
                    }
                }
                return null;
            }
        }
        private readonly string[] _validatedProperties = { "Datum", "Adresa", "Indeks", "EmailAdresa", "KontaktTelefon","Ime","Prezime","TrenutnaGodinaStudija","GodinaUpisa" };

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
        public Student ToStudent()
         {
            adresa.setAdresa(adresaStanovanja);
            datum.setDatum(datumRodjenja);
            indeks.SetIndex(brojIndeksa);
             return new Student(id,prezime, ime, datum, adresa, kontaktTelefon,
                emailAdresa, indeks, trenutnaGodinaStudija.ToString(),status,prosecnaOcena);
         }

        public StudentDTO(StudentskaSluzbaDAO ssluzba)
        {
            this.ssluzba = ssluzba;
            prosecnaOcena = 0.00;
            adresa = new Adresa();
            datum = new Datum();
            indeks= new Indeks();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public StudentDTO(Student s,StudentskaSluzbaDAO ssluzba=null)
        {
            this.ssluzba = ssluzba;
            adresa = new Adresa();
            datum = new Datum();
            indeks=new Indeks();
            ime = s.ime;
            prezime = s.prezime;
            id = s.id;
            trenutnaGodinaStudija = int.Parse(s.trenutnaGodinaStudija);
            status = s.status;
            prosecnaOcena = s.prosecnaOcena;
            datumRodjenja = s.datumRodjena.ToString();
            adresaStanovanja = s.adresaStanovanja.ToString();
            kontaktTelefon=s.kontaktTelefon;
            emailAdresa = s.emailAdresa;
            brojIndeksa=s.brojIndeksa.ToString();
            godinaUpisa =brojIndeksa.Split('/')[1];
            Debug.Write(godinaUpisa);

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
