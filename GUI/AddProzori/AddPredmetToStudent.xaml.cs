using CLI.DAO;
using CLI.Model;
using CLI.Observer;
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
    /// Interaction logic for AddPredmetToStudent.xaml
    /// </summary>
    public partial class AddPredmetToStudent : Window,IObserver
    {
        public ObservableCollection<PredmetAddToStudentDTO> Predmeti { get; set; }
        public StudentskaSluzbaDAO ssluzba;
        private int idSelected = -1;
        int studentId;
        public AddPredmetToStudent(StudentskaSluzbaDAO ssluzba,int id)
        {
            studentId = id;
            this.ssluzba= ssluzba;
            DataContext = this;
            Predmeti = new ObservableCollection<PredmetAddToStudentDTO>();
            
            InitializeComponent();
            Update();
        }

        public void Update()
        {
            Predmeti.Clear();


            ssluzba.GetAllPredmeti().ForEach(p => {
                if (p.poloziliPredmet.FindIndex(pp => pp.id == studentId) == -1)
                    if (p.nisuPoloziliPredmet.FindIndex(pnp=>pnp.id==studentId) == -1)
                    {if(p.godinaStudija<=int.Parse(ssluzba.GetStudentById(studentId).trenutnaGodinaStudija))
                        Predmeti.Add(new PredmetAddToStudentDTO(p));
                    }
            });

        }

        private void Add(object sender, RoutedEventArgs e)
        {
            if (idSelected != -1)
            {
                ssluzba.AddStudentPredmet(new StudentPredmet(studentId, idSelected));
                Close();
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (PredmetAddToStudentDTO selectedItem in e.AddedItems)
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
