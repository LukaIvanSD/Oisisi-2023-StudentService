using CLI.DAO;
using CLI.Model;
using GUI.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Polaganje.xaml
    /// </summary>
    public partial class Polaganje : Window
    {
        public int idStudenta { get; set; }
        public PolozeniDTO predmet { get; set; }
        private NePolozeniDTO nepolozeni { get; set; }

        /* public String datumPolaganja { get; set; }

         public string DatumPolaganja
         {
             get
             {
                 return datumPolaganja;
             }
             set
             {
                 if (value != datumPolaganja)
                 {
                     if(ssluzba.DatumRegex(value))
                     {
                         datumPolaganja = value;
                         OnPropertyChanged("DatumPolaganja");
                     }
                 }
             }
         }
         public string ocena { get; set; }

         public String Ocena
         {
             get
             {
                 return ocena;
             }
             set
             {
                 if (value != ocena)
                 {
                     OcenaPovera();
                     Debug.WriteLine("SLOVA" + ocena);
                     OnPropertyChanged("Ocena");
                 }
             }
         }

         private void OcenaPovera()
         {
             switch (comboBox1.SelectedIndex)
             {
                 case 0:
                     ocena = "6";
                     break;
                 case 1:
                     ocena = "7";
                     break;
                 case 2:
                     ocena = "8";
                     break;
                 case 3:
                     ocena = "9";
                     break;
                 case 4:
                     ocena = "10";
                     break;
             }
         }*/

        public StudentskaSluzbaDAO ssluzba { get; set; }
        public Polaganje(NePolozeniDTO p, int idStudent, StudentskaSluzbaDAO ssluzb)
        {
            predmet=new PolozeniDTO(ssluzb);
            nepolozeni = p;
            predmet.Sifra = p.Sifra;//PROVERITI DA LI RADI OVA LINIJA
            predmet.Naziv = p.Naziv;
            predmet.Espb = p.Espb;
            idStudenta = idStudent;
            ssluzba = ssluzb;
            InitializeComponent();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ADD(object sender, RoutedEventArgs e)
        {
            if (predmet.IsValid)
            {
                Datum datum = new Datum();
                datum.setDatum(predmet.Datum);
                ssluzba.AddOcena(new Ocena(idStudenta, nepolozeni.id,predmet.Ocena, datum));
                ssluzba.izracunajProsek(idStudenta);
                Close();
            }
            else
            {
                MessageBox.Show("Course can not be created. Not all fields are valid.");
            }
           
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.B)
            { e.Handled = true; MainWindow.ChangeToEnglish(); }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.R)
            { e.Handled = true; MainWindow.ChangeToSerbian(); }
        }
    }
}
