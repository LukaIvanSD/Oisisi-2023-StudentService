using CLI.DAO;
using CLI.Model;
using GUI.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ShowProfessorsForKatedra.xaml
    /// </summary>
    public partial class ShowProfessorsForKatedra : Window
    {
        
        private StudentskaSluzbaDAO ssluzba;

        public KatedraDTO katedra { get; set; }
        int idSelected = -1;
        public ObservableCollection<ShowProfessorsForStudentDTO> profesori { get; set; }

        public ShowProfessorsForKatedra(KatedraDTO katedra, StudentskaSluzbaDAO ssluzba)
        {
            profesori = new ObservableCollection<ShowProfessorsForStudentDTO>();
            this.katedra = katedra;
            this.ssluzba = ssluzba;
            
            InitializeComponent();
            prikaziProfesore();
            PostaviZaSefa.IsEnabled = false;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public ShowProfessorsForKatedra()
        {
            InitializeComponent();
        }

        private void prikaziProfesore()
        {
            profesori.Clear();
            foreach (Profesor p in ssluzba.GetKatedraById(katedra.id).profesori)
            {
                profesori.Add(new ShowProfessorsForStudentDTO(p));
            }
        }
        private bool daLiMozeBitiSef(int idProf, KatedraDTO k)
        {
            Profesor p = ssluzba.GetProfessorById(idProf);

            if (int.Parse(p.godineStaza) >= 5 && (p.zvanje.ToLower().Equals("redovni profesor") || p.zvanje.ToLower().Equals("vanredni")))
            {
                k.SefId = idProf;
                return true;
            }

            return false;
        }
        private void PostaviSefa(object sender, RoutedEventArgs e)
        {//refactor my function and add validation function
            if(daLiMozeBitiSef(idSelected, katedra))
            {
                MessageBox.Show("Uspesno postavljen sef");
                Close();
            }
            else
            {
                MessageBox.Show("Izabrani profesor ne zadovoljava kriterijume za potencijalnog sefa katedre!");
            }
        }
        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            PostaviZaSefa.IsEnabled = true;
            foreach (ShowProfessorsForStudentDTO selectedItem in e.AddedItems)
            {
                idSelected = selectedItem.id;
                Debug.WriteLine("SELEKTOVAO SI :" + idSelected);
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
