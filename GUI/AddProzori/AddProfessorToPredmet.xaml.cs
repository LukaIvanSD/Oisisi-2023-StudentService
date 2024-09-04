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
using CLI.DAO;
using CLI.Model;
using CLI.Observer;
using GUI.DTO;
namespace GUI
{
    /// <summary>
    /// Interaction logic for AddProfessorToPredmet.xaml
    /// </summary>
    public partial class AddProfessorToPredmet : Window,IObserver
    {
        public ObservableCollection<ProfessorAddToPredmetDTO> Profesori { get; set; }
        private StudentskaSluzbaDAO ssluzba;
        private int idPredmeta;
        int idSelected = -1;
        private PredmetDTO predmet;

        public AddProfessorToPredmet(StudentskaSluzbaDAO ssluzba,int idPredmeta, PredmetDTO p)
        {
            predmet = p;
            DataContext = this;
            this.idPredmeta = idPredmeta;
            this.ssluzba = ssluzba;
            Profesori = new ObservableCollection<ProfessorAddToPredmetDTO>();
            InitializeComponent();
            ssluzba.PredmetSubject.Subscribe(this);
            Closing += AddProfessorToPredmet_Closing;
            Update();
        }
        private void AddProfessorToPredmet_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ssluzba.PredmetSubject.Unsubscribe(this);
        }
        public void Update()
        {
            Profesori.Clear();
            foreach (Profesor prof in ssluzba.GetAllProfessors())
            {
                    Profesori.Add(new ProfessorAddToPredmetDTO(prof));
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (ProfessorAddToPredmetDTO selectedItem in e.AddedItems)
            {
                idSelected = selectedItem.id;
                Debug.WriteLine("SELEKTOVAO SI :" + idSelected);
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            if(idSelected != -1)
            {
                ssluzba.AddPredmetToProfessor(idPredmeta, idSelected);
                predmet.IdProf = idSelected;
                ssluzba.PredmetSubject.Unsubscribe(this);
                Close();
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            ssluzba.PredmetSubject.Unsubscribe(this);
            Close();
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
