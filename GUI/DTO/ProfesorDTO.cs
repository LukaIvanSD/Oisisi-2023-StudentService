using CLI.DAO;
using CLI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GUI.DTO
{
    public class ProfesorDTO: INotifyPropertyChanged, IDataErrorInfo
    {
        private Datum datum;
        private Adresa adresa;
        private StudentskaSluzbaDAO ssluzba;
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
        private string zvanje;
        public string Zvanje
        {
            get
            {
                return zvanje;
            }
            set
            {
                if (value != zvanje)
                {
                    zvanje = value;
                    OnPropertyChanged("Zvanje");
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
        private string datumRodjenja;
        public string DatumRodjenja
        {
            get
            {
                return datumRodjenja;
            }
            set
            {
                if (value != datumRodjenja)
                {
                    datumRodjenja = value;
                    OnPropertyChanged("DatumRodjenja");
                }
            }
        }
        private string adresaStanovanja;
        public string AdresaStanovanja
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
                    OnPropertyChanged("AdresaStanovanja");
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
        private string brojLicneKarte;
        public string BrojLicneKarte
        {
            get
            {
                return brojLicneKarte;
            }
            set
            {
                if (value != brojLicneKarte)
                {
                    brojLicneKarte = value;
                    OnPropertyChanged("BrojLicneKarte");
                }
            }
        }
        private string godineStaza;
        public string GodineStaza
        {
            get
            {
                return godineStaza;
            }
            set
            {
                if (value != godineStaza)
                {
                    godineStaza = value;
                    OnPropertyChanged("GodineStaza");
                }
            }
        }
        private int idKatedre;
        public int IdKatedre
        {
            get
            {
                return idKatedre;
            }
            set
            {
                if (value != idKatedre)
                {
                    idKatedre = value;
                    OnPropertyChanged("IdKatedre");
                }
            }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "DatumRodjenja")
                {
                    if (string.IsNullOrEmpty(datumRodjenja))
                        return "Date of birth is required";
                    if (!ssluzba.DatumRegex(datumRodjenja))
                         return "Format not good. Try again.";
                }
                else if (columnName == "AdresaStanovanja")
                {
                    if (string.IsNullOrEmpty(adresaStanovanja))
                        return "Adress is required";
                     if (!ssluzba.AdresaRegex(adresaStanovanja))
                      return "Format not good. Try again.";
                }
                else if (columnName == "IdKatedre")
                {
                    if (IdKatedre < 0)
                        return "Department ID cannot be negative number";
                    if (ssluzba.GetAllKatedre().FindIndex(k => k.id == idKatedre) == -1)
                        return "Non existant department";
                }
                else if (columnName == "Ime")
                {
                    if (string.IsNullOrEmpty(Ime))
                        return "Name is required";
     
                }
                else if (columnName == "Prezime")
                {
                    if (string.IsNullOrEmpty(Prezime))
                        return "Surname is required";
            
                }
                else if (columnName == "BrojLicneKarte")
                {
                    if (string.IsNullOrEmpty(BrojLicneKarte))
                        return "ID number is required";
           
                }
                else if (columnName == "EmailAdresa")
                {
                    if (string.IsNullOrEmpty(EmailAdresa))
                        return "E-mail adress is required";
           
                }
                else if (columnName == "KontaktTelefon")
                {
                    if (string.IsNullOrEmpty(KontaktTelefon))
                        return "Phone number is required";
         
                }
                else if (columnName == "Zvanje")
                {
                    if (string.IsNullOrEmpty(Zvanje))
                        return "Title is required";
     
                }
                else if (columnName == "GodineStaza")
                {
                    if (string.IsNullOrEmpty(godineStaza))
                        return "Years of working is required";
                    if (!int.TryParse(godineStaza, out int intValue))
                        return "Must be a number";
                    else
                    if (int.Parse(godineStaza) < 0)
                        return "Not valid.";

       
                }
                return null;
            }
        }
        private readonly string[] _validatedProperties = { "DatumRodjenja", "AdresaStanovanja","IdKatedre","Ime","Prezime", "BrojLicneKarte", "KontaktTelefon", "EmailAdresa","Zvanje", "GodineStaza" };

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

        public Profesor ToProfesor()
        {
            datum.setDatum(datumRodjenja);
            adresa.setAdresa(adresaStanovanja);
            return new Profesor(id,ime, prezime,datum,
                    adresa, kontaktTelefon, emailAdresa, brojLicneKarte, zvanje, godineStaza, idKatedre);
        }

        public ProfesorDTO(StudentskaSluzbaDAO ssluzba)
        {
            this.ssluzba = ssluzba;
            datum = new Datum();
            adresa = new Adresa();
            this.ssluzba = ssluzba;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ProfesorDTO(Profesor p,StudentskaSluzbaDAO ssluzba=null)
        {
            this.ssluzba = ssluzba;
            datum = new Datum();
            adresa = new Adresa();
            ime = p.ime;
            prezime = p.prezime;
            id = p.id;
            zvanje = p.zvanje;
            emailAdresa = p.emailAdresa;
            datumRodjenja = p.datumRodjenja.ToString();
            adresaStanovanja = p.adresaStanovanja.ToString();
            kontaktTelefon= p.kontaktTelefon;
            brojLicneKarte =p.brojLicneKarte;
            idKatedre = p.idKatedre;
            godineStaza = p.godineStaza;
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
