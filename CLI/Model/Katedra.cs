using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLI.Serialization;
namespace CLI.Model
{
    public  class Katedra : ISerializable
    {
        public int id { get; set; }
        public string sifra { get; set; }
        public string naziv { get; set; }
        public int sefId { get; set; }
        public Profesor? sef { get; set; }
        public List<Profesor> profesori { get; set; }   // ne serijalizujemo
        
        public Katedra() { }

        public Katedra(string sifra, string naziv, int sefId)
        {
            this.sifra = sifra;
            this.naziv = naziv;
            this.sefId = sefId;
            this.profesori = new List<Profesor>();
        }
        public Katedra(int id, string sifra, string naziv, int sefId)
        {
            this.id = id;
            this.sifra = sifra;
            this.naziv = naziv;
            this.sefId = sefId;
            this.profesori = new List<Profesor>();
        }
        public override string ToString()
        {
            if(sef!=null)
            return $"{id,10}|{sifra,10}|{naziv,15}|{sefId,15}|{ispisiSefa(sef),30}" +
                $"|{listaProfesora(profesori),50}|";
            else
                return $"{id,10}|{sifra,10}|{naziv,15}|{sefId,15}| Nema sefa" +
               $"|{listaProfesora(profesori),50}|";
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
            sifra,naziv,sefId.ToString()
        };
            return csvValues;
        }

        public string ispisiSefa(Profesor sef)
        {
            string sefString = "";
            if (sef == null)
                return "Nema sefa";
            sefString = sef.ime.ToString() + " " + sef.prezime.ToString();
            return sefString;
        }

        public void FromCSV(string[] values)
        {
            sifra =values[0];
            naziv = values[1];
            sefId = int.Parse(values[2]);
        }

        public string listaProfesora(List<Profesor> profesori)
        {
            string a = "";
            if (profesori == null || profesori.Count() == 0)
            {
                return a;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < profesori.Count; i++)
            {
                sb.Append($"{profesori[i].ime} {profesori[i].prezime}");

                if (i < profesori.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            a = sb.ToString();
            return a;

        }

        /*public override string ToString()
        {
            return $"Naziv: {naziv} | Sef: {sef.ime} {sef.prezime} | {listaProfesora(profesori)}";
        }*/
    }
}
