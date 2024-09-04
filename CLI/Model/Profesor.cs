using CLI.Serialization;
using System.Diagnostics;
using System.Text;

namespace CLI.Model
{
    public  class Profesor : ISerializable
    {
        public int id { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public Datum datumRodjenja { get; set; }
        public Adresa adresaStanovanja { get; set; }
        public string kontaktTelefon { get; set; }
        public string emailAdresa { get; set; }
        public string brojLicneKarte { get; set; }
        public string zvanje { get; set; }
        public string godineStaza { get; set; }
        public int idKatedre { get; set; }//BITNO 
        public List<Predmet> predaje{get; set;}

        //DA LI TREBA DA IMAMO ID PREDMETA KOJI ON PREDAJE?????

        public Profesor() { }
        public Profesor(string ime, string prezime, Datum datumRodjenja, Adresa adresaStanovanja,
            string kontaktTelefon, string emailAdresa, string brojLicneKarte, string zvanje, string godineStaza, int idKatedre)
        {
            this.ime = ime;
            this.prezime = prezime;
            this.datumRodjenja = datumRodjenja;
            this.adresaStanovanja = adresaStanovanja;
            this.kontaktTelefon = kontaktTelefon;
            this.emailAdresa = emailAdresa;
            this.brojLicneKarte = brojLicneKarte;
            this.zvanje = zvanje;
            this.godineStaza = godineStaza;
            this.idKatedre = idKatedre;
        }
        public Profesor(int id,string ime, string prezime, Datum datumRodjenja, Adresa adresaStanovanja,
            string kontaktTelefon, string emailAdresa, string brojLicneKarte, string zvanje, string godineStaza, int idKatedre)
        {
            this.id= id;
            this.ime = ime;
            this.prezime = prezime;
            this.datumRodjenja = datumRodjenja;
            this.adresaStanovanja = adresaStanovanja;
            this.kontaktTelefon = kontaktTelefon;
            this.emailAdresa = emailAdresa;
            this.brojLicneKarte = brojLicneKarte;
            this.zvanje = zvanje;
            this.godineStaza = godineStaza;
            this.idKatedre = idKatedre;
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
            ime, prezime, datumRodjenja.ToString() ,adresaStanovanja.ToString(), kontaktTelefon, emailAdresa, brojLicneKarte, zvanje, godineStaza,idKatedre.ToString()  
        };
            return csvValues;
        }

        public string listapredmeta() {
            string a = "";
            if (predaje == null || predaje.Count == 0)
            {
                return a;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < predaje.Count; i++)
            {
                sb.Append(predaje[i].naziv);

                if (i < predaje.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            a = sb.ToString();
            return a;

        }

        public override string ToString()
        {
            return $"{id,5}|{ime,8}|{prezime,12}|{datumRodjenja,15}|{adresaStanovanja,30}" +
                $"|{kontaktTelefon,12}|{emailAdresa,20}|{brojLicneKarte,12}|{zvanje,20}|" +
                $"{godineStaza,11}|{idKatedre,11}|{listapredmeta(),40}|";
        }
        public void FromCSV(string[] values)
        {
            ime = values[0];
            prezime = values[1];
            datumRodjenja = new Datum(values[2]);
            adresaStanovanja = new Adresa(values[3]);
            kontaktTelefon =values[4];
            emailAdresa = values[5];
            brojLicneKarte = values[6];
            zvanje= values[7];
            godineStaza = values[8];
            idKatedre = int.Parse(values[9]);
        }
    }
}
