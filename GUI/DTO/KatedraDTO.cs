using CLI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.DTO
{
    public class KatedraDTO:INotifyPropertyChanged,IDataErrorInfo
    {
        public int id { get; set; }

        private string sifra;
        public string Sifra
        {
            get
            {
                return sifra;
            }
            set
            {
                if (value != sifra)
                {
                    sifra = value;
                    OnPropertyChanged("Sifra");
                }
            }
        }
        private string naziv;
        public string Naziv
        {
            get
            {
                return naziv;
            }
            set
            {
                if (value != naziv)
                {
                    naziv = value;
                    OnPropertyChanged("Naziv");
                }
            }
        }
        private int sefId;
        public int SefId
        {
            get
            {
                return sefId;
            }
            set
            {
                if (value != sefId)
                {
                    sefId = value;
                    OnPropertyChanged("SefId");
                }
            }
        }
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Naziv")
                {
                    if (string.IsNullOrEmpty(Naziv))
                    {
                        return "Not valid";
                    }
                }
                if (columnName == "Sifra")
                {
                    if (string.IsNullOrEmpty(Sifra))
                        return "Not valid";
                }
                return null;
            }
        }
        private readonly string[] _validatedProperties = {"Naziv","Sifra" };

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

        public Katedra ToKatedra()
        {
            return new Katedra(id, sifra, naziv,sefId);
        }

        public KatedraDTO()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public KatedraDTO(Katedra k)
        {
            naziv = k.naziv;
            sifra = k.sifra;
            id = k.id;
            sefId = k.sefId;
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
