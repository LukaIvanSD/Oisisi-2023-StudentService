using CLI.Serialization;
using System.Xml.Linq;

namespace CLI.Model
{
    public  class Ocena : ISerializable
    {
        public int id { get; set; }
        public int idStudentPolozio { get; set; }//BITNO 
        public int idPredmet { get; set; }//BITNO
        public int ocena { get; set; }//BITNO ZA RACUNANJE PROSEKA INACE NE
        public Datum datumPolaganja { get; set; }

        public Ocena() { }
        public Ocena(int idStudentPolozio, int idPredmet, int ocena, Datum datumPolaganja) 
        {
            this.idStudentPolozio = idStudentPolozio;
            this.idPredmet = idPredmet;
            this.ocena = ocena;
            this.datumPolaganja = datumPolaganja;
        }
        public override string ToString()
        {
            return $"{id,12}|{idStudentPolozio,15}|{idPredmet,15}|{ocena,12}" +
                $"|{datumPolaganja,16}|";
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
            idStudentPolozio.ToString(),idPredmet.ToString(),ocena.ToString(),datumPolaganja.ToString()
        };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            idStudentPolozio = int.Parse(values[0]);
            idPredmet = int.Parse(values[1]);
            ocena = int.Parse(values[2]);
            datumPolaganja=new Datum(values[3]);


        }
    }
}
