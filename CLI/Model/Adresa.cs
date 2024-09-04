using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CLI.Model
{
    public  class Adresa
    {
        string ulica;
        int broj;
        string grad;
        string drzava;

        public Adresa(string ulica, int broj, string grad, string drzava)
        {
            this.ulica = ulica;
            this.broj = broj;
            this.grad = grad;
            this.drzava = drzava;
        }

        public Adresa()
        {
        }
        public void setAdresa(string values) {
            //Radosava Mirkovica 1 Smederevo, Srbija
            string pattern = @"^([0-9.ČĆŠĐŽćčšđžA-Za-z\s]+) (\d{1,3}) ([ČĆŠĐŽćčšđžA-Za-z\s]+),([ČĆŠĐŽćčšđžA-Za-z\s]+)$"; 
            Match match = Regex.Match(values, pattern);
           
            this.ulica = match.Groups[1].Value;
            Debug.WriteLine(match.Groups[2].Value);
            this.broj = int.Parse(match.Groups[2].Value);
            Debug.WriteLine("Broj"+broj);
            this.grad = match.Groups[3].Value;
            this.drzava = match.Groups[4].Value;
        }
        public Adresa(string values)
        {
            // morace da si izmeni radi samo sa atributima od jedne reci
            setAdresa(values);
        }

        public override string ToString()
        {
            return $"{ulica} {broj} {grad},{drzava}";
        }
    }
}
