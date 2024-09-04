using CLI.Serialization;
using System.Globalization;
using System.Text;

namespace CLI.Model
{
   public  enum Status
    {
        B,          // Budzetski student
        S           // Samofinansirajuci student
    }


    public  class Student : ISerializable
    {
        public int id { get; set; }
        public string prezime { get; set; }
        public string ime { get; set; }
        public Datum datumRodjena { get; set; }
        public Adresa adresaStanovanja { get; set; }
        public string kontaktTelefon { get; set; }
        public string emailAdresa { get; set; }
        public Indeks brojIndeksa { get; set; }
        public string trenutnaGodinaStudija { get; set; }
        public Status status { get; set; }
        public double prosecnaOcena { get; set; }//NIJE REsENO
        public List<Predmet> nepolozeniIspiti { get; set; }// ne unosimo, predmet RESENO
        public List <Predmet> polozeniIspiti { get; set; }// ne unosimo, RESENO
        
        public Student()
        {
        }

        public Student(int id, string prezime, string ime, Datum datumRodjena, Adresa adresaStanovanja,
            string kontaktTelefon, string emailAdresa, Indeks brojIndeksa, string trenutnaGodinaStudija,
            Status status,double prosecnaOcena)
        {
            this.id = id;
            this.prezime = prezime;
            this.ime = ime;
            this.datumRodjena = datumRodjena;
            this.adresaStanovanja = adresaStanovanja;
            this.kontaktTelefon = kontaktTelefon;
            this.emailAdresa = emailAdresa;
            this.brojIndeksa = brojIndeksa;
            this.trenutnaGodinaStudija = trenutnaGodinaStudija;
            this.status = status;
            this.prosecnaOcena = prosecnaOcena;
        }

        public Student(string prezime, string ime, Datum datumRodjena, Adresa adresaStanovanja,
            string kontaktTelefon, string emailAdresa, Indeks brojIndeksa, string trenutnaGodinaStudija,
            Status status)
        {
            this.prezime = prezime;
            this.ime = ime;
            this.datumRodjena = datumRodjena;
            this.adresaStanovanja = adresaStanovanja;
            this.kontaktTelefon = kontaktTelefon;
            this.emailAdresa = emailAdresa;
            this.brojIndeksa = brojIndeksa;
            this.trenutnaGodinaStudija = trenutnaGodinaStudija;
            this.status = status;
            this.prosecnaOcena = 0;
            this.nepolozeniIspiti = new List<Predmet>();
            this.polozeniIspiti = new List<Predmet>();
        }
        public string listaNepolozenih(List<Predmet> nepolozeniIspiti)
        {
            string a = "";
            if (nepolozeniIspiti == null || nepolozeniIspiti.Count == 0)
            {
                return a;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < nepolozeniIspiti.Count; i++)
            {
                sb.Append(nepolozeniIspiti[i].naziv);

                if (i < nepolozeniIspiti.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            a = sb.ToString();
            return a;

        }

        public string listaPolozenih(List<Predmet> polozeniIspiti)
        {
            string a = "";
            if (polozeniIspiti == null || polozeniIspiti.Count == 0)
            {
                return a;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < polozeniIspiti.Count; i++)
            {
                sb.Append(polozeniIspiti[i].naziv);

                if (i < polozeniIspiti.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            a = sb.ToString();
            return a;

        }
        public override string ToString()
        {
            return $"{id,4}|{ime, 8}|{prezime,12}|{datumRodjena, 15}|{adresaStanovanja, 30}|" +
                $"{kontaktTelefon, 12}|{emailAdresa, 20}|{brojIndeksa, 12}|{trenutnaGodinaStudija, 7}|" +
                $"{status, 8}|{prosecnaOcena, 8}|{listaNepolozenih(nepolozeniIspiti),30}|{ listaPolozenih(polozeniIspiti),30}|";
        }

        public void FromCSV(string[] values)
        {
            ime = values[0];
            prezime = values[1];
            datumRodjena = new Datum(values[2]);
            adresaStanovanja = new Adresa(values[3]);
            kontaktTelefon = values[4];
            emailAdresa = values[5];
            brojIndeksa = new Indeks(values[6]);
            trenutnaGodinaStudija = values[7];
            if (values[8] == "B") status = Status.B;
            else status = Status.S;
            prosecnaOcena = double.Parse(values[9]);
            prosecnaOcena = double.Parse(values[9], CultureInfo.InvariantCulture);

        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
            ime,
            prezime,
            datumRodjena.ToString(),
            adresaStanovanja.ToString(),
            kontaktTelefon,
            emailAdresa,
            brojIndeksa.ToString(),
            trenutnaGodinaStudija,
            status.ToString(),
            prosecnaOcena.ToString("0.##", CultureInfo.InvariantCulture)
        };
            return csvValues;
        }
    }
}
