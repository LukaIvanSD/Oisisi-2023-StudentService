using CLI.Serialization;
using System.Diagnostics;
using System.Text;

namespace CLI.Model
{
    public enum Semestar
    {
        Letnji,
        Zimski
    }
    public class Predmet : ISerializable
    {
        public int id { get; set; }
        public int sifra { get; set; }
        public string naziv { get; set; }//BITNO
        public Semestar semestar { get; set; }
        public int godinaStudija { get; set; }

        // Id professora se serijalizuje
        public int professorId { get; set; }//BITNO

        // Professor se ne serijalizuje
        public Profesor? profesor { get; set; }

        public int espb { get; set; }
        public List<Student> poloziliPredmet { get; set; }//RESENO
        public List<Student> nisuPoloziliPredmet { get; set; }//RESENO

        public Predmet() { }

        public Predmet(int sifra, string naziv, Semestar semestar, int godinaStudija,
            int professorId, int espb)
        {
            this.sifra = sifra;
            this.naziv = naziv;
            this.semestar = semestar;
            this.godinaStudija = godinaStudija;
            this.professorId = professorId;
            this.profesor = profesor;
            this.espb = espb;
            this.poloziliPredmet = new List<Student>();
            this.nisuPoloziliPredmet = new List<Student>();
        }
        public Predmet(int id,int sifra, string naziv, Semestar semestar, int godinaStudija,
        int professorId, int espb)
        {
            this.id = id;
            this.sifra = sifra;
            this.naziv = naziv;
            this.semestar = semestar;
            this.godinaStudija = godinaStudija;
            this.professorId = professorId;
            this.profesor = profesor;
            this.espb = espb;
            this.poloziliPredmet = new List<Student>();
            this.nisuPoloziliPredmet = new List<Student>();
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
            sifra.ToString(),naziv,semestar.ToString(),godinaStudija.ToString(),espb.ToString(),professorId.ToString()
        };
            return csvValues;
        }

        public string polozili() {
            string a = "";
            if (poloziliPredmet == null || poloziliPredmet.Count == 0)
            {
                return a;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < poloziliPredmet.Count; i++)
            {
                sb.Append(poloziliPredmet[i].id);

                if (i < poloziliPredmet.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            a = sb.ToString();
            return a;

        }
        public string nisupolozili() {
            string a = "";
            Console.WriteLine("AA"+nisuPoloziliPredmet.Count());
            if (nisuPoloziliPredmet == null || nisuPoloziliPredmet.Count == 0)
            {
                return a;
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nisuPoloziliPredmet.Count; i++)
            {               
                sb.Append(nisuPoloziliPredmet[i].id);

                if (i < nisuPoloziliPredmet.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            a = sb.ToString();
            return a;

        }
        public override string ToString()
        {

            if (professorId != -1 && profesor != null)
            {
                return $"{id,6}|{sifra,8}|{naziv,15}|{semestar,10}|{godinaStudija,13}|" +
                $"{espb,6}|" + $"{professorId,10}|{profesor.ime + " " + profesor.prezime,20}|{polozili(),30}|" + $"{nisupolozili(),30}";
            }
            else
            {
                return $"{id,6}|{sifra,8}|{naziv,15}|{semestar,10}|{godinaStudija,13}|" +
                $"{espb,6}|" + $"{professorId,10}|{"Nema profesora ",20}|{polozili(),30}|" + $"{nisupolozili(),30}";
            }
        }
        public void FromCSV(string[] values)
        {
            sifra = int.Parse(values[0]);
            naziv= values[1];
            if(Enum.TryParse<Semestar>(values[2],out Semestar semestar));
            this.semestar = semestar;
            godinaStudija = int.Parse(values[3]);
            espb = int.Parse(values[4]);
            professorId = int.Parse(values[5]); 

        }

    }
}
