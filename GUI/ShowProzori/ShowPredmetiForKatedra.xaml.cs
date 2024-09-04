using CLI.DAO;
using CLI.Model;
using GUI.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ShowPredmetiForKatedra.xaml
    /// </summary>
    public partial class ShowPredmetiForKatedra : Window
    {

        private StudentskaSluzbaDAO ssluzba;

        public KatedraDTO katedra {  get; set; }

        public ObservableCollection<ShowPredmetiForKatedraDTO> predmeti { get; set; }

        public ShowPredmetiForKatedra(KatedraDTO katedra, StudentskaSluzbaDAO ssluzba)
        {
            predmeti = new ObservableCollection<ShowPredmetiForKatedraDTO>();
            this.katedra = katedra;
            this.ssluzba = ssluzba;

            InitializeComponent();
            PrikaziPredmete();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PrikaziPredmete()
        {
            predmeti.Clear();
            foreach (Predmet p in ssluzba.GetPredmetiForKatedra(katedra.id))
            {
                predmeti.Add(new ShowPredmetiForKatedraDTO(p));
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
