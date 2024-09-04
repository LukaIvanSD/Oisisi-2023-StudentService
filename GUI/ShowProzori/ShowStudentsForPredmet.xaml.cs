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
    /// Interaction logic for ShowStudentsForPredmet.xaml
    /// </summary>
    public partial class ShowStudentsForPredmet : Window
    {
        private StudentskaSluzbaDAO ssluzba;
        private List<PredmetDTO> predmeti { get; set; }
        public ObservableCollection<ShowStudentsForProfessorDTO> Studenti { get; set; }
        public ObservableCollection<ShowStudentsForProfessorDTO> StudentiPolozili { get; set; }
        public ShowStudentsForPredmet(List<PredmetDTO> predmeti, StudentskaSluzbaDAO ssluzba)
        {
            Studenti = new ObservableCollection<ShowStudentsForProfessorDTO>();
            StudentiPolozili = new ObservableCollection<ShowStudentsForProfessorDTO>();
            this.predmeti=predmeti;
            this.ssluzba=ssluzba;
            InitializeComponent();
            PrikaziStudenteZaPredmete();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void PrikaziStudenteZaPredmete()
        {
            Studenti.Clear();
            ssluzba.GetAllStudents().ForEach(s =>
            {
                if (s.nepolozeniIspiti.FindIndex(snp => snp.id == predmeti[0].id || snp.id == predmeti[1].id) != -1)
                {
                    Studenti.Add(new ShowStudentsForProfessorDTO(s));
                }
            });
            StudentiPolozili.Clear();
            foreach (var student in ssluzba.GetAllStudents())//Valjda radi funkcija
            {
                bool hasPassedFirstSubject = student.polozeniIspiti.Any(sp => sp.id == predmeti[0].id);
                bool hasPassedSecondSubject = student.polozeniIspiti.Any(sp => sp.id == predmeti[1].id);
                bool hasNotFailedOtherSubject = student.nepolozeniIspiti.Any(snp => snp.id == predmeti[0].id || snp.id == predmeti[1].id);
                if ((hasPassedFirstSubject && hasNotFailedOtherSubject) || (hasPassedSecondSubject && hasNotFailedOtherSubject))
                {
                    StudentiPolozili.Add(new ShowStudentsForProfessorDTO(student));
                }
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
