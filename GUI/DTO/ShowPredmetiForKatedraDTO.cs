
using CLI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.DTO
{
    public class ShowPredmetiForKatedraDTO
    {
        public int id { get; set; }
        public string ime { get; set; }

        public ShowPredmetiForKatedraDTO() { }

        public ShowPredmetiForKatedraDTO(Predmet p)
        {
            id = p.id;
            ime = p.sifra + " " + p.naziv;
        }
    }
}
