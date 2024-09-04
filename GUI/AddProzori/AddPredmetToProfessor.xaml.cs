using CLI.DAO;
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
    /// Interaction logic for AddPredmetToProfessor.xaml
    /// </summary>
    public partial class AddPredmetToProfessor : Window,IObserver
    {
       public  ObservableCollection<PredmetAddToStudentDTO>Predmeti { get; set; }
        public StudentskaSluzbaDAO ssluzba;
        private int idSelected = -1;
        private int professorId;
        public AddPredmetToProfessor(StudentskaSluzbaDAO ssluzba, int id)
        {
            InitializeComponent();
            professorId = id;
            this.ssluzba = ssluzba;
            DataContext = this;
            Predmeti = new ObservableCollection<PredmetAddToStudentDTO>();
            ssluzba.PredmetSubject.Subscribe(this);
            Closing += AddPredmetToProfessor_Closing;
            Update();
        }
        private void AddPredmetToProfessor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ssluzba.PredmetSubject.Unsubscribe(this);
        }
        private void Add(object sender, RoutedEventArgs e)
        {
            //implementirati funkciju za dodavanje predmeta profesoru i videti Subscribe !!!!!
            if (idSelected != -1)
            {
                ssluzba.AddPredmetToProfessor(idSelected, professorId);
                ssluzba.PredmetSubject.Unsubscribe(this);
                Close();
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            ssluzba.PredmetSubject.Unsubscribe(this);
            Close();
        }

        public void Update()
        {
            Predmeti.Clear();
            ssluzba._predmeti.ForEach(p => {
                if (p.professorId == -1)//PROVERITI OVAJ USLOV
                {
                    Predmeti.Add(new PredmetAddToStudentDTO(p));
                }
            });
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
