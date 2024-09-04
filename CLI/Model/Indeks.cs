using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Model
{
    public  class Indeks
    {
        public string oznakaSmera { get; set; }
        public int brojUpisa { get; set; }
        public int godinaUpisa { get; set; }

        public Indeks() { }
        public void SetIndex(string values) {
            string[] indeksVrednosti1 = values.Split(' ');
            this.oznakaSmera = indeksVrednosti1[0];
            string[] indeksVrednosti2 = indeksVrednosti1[1].Split('/');
            this.brojUpisa = int.Parse(indeksVrednosti2[0]);
            this.godinaUpisa = int.Parse(indeksVrednosti2[1]);
        }
        public Indeks(string oznakaSmera, int brojUpisa, int godinaUpisa)
        {
            this.oznakaSmera = oznakaSmera;
            this.brojUpisa = brojUpisa;
            this.godinaUpisa = godinaUpisa;
        }

        public Indeks(string values)
        {
            string[] indeksVrednosti1 = values.Split(' ');
            this.oznakaSmera = indeksVrednosti1[0];
            string[] indeksVrednosti2 = indeksVrednosti1[1].Split('/');
            this.brojUpisa = int.Parse(indeksVrednosti2[0]);
            this.godinaUpisa = int.Parse(indeksVrednosti2[1]);
        }
        public override string ToString()
        {
            return $"{oznakaSmera} {brojUpisa}/{godinaUpisa}";
        }
    }
}
