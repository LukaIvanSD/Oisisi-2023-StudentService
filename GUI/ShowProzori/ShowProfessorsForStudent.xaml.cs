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
    /// Interaction logic for ShowProfessorsForStudent.xaml
    /// </summary>
    public partial class ShowProfessorsForStudent : Window
    {
        private StudentskaSluzbaDAO ssluzba;

        public StudentDTO student { get; set; }

        public ObservableCollection<ShowProfessorsForStudentDTO> profesori { get; set; }

        public ShowProfessorsForStudent(StudentDTO s, StudentskaSluzbaDAO ssluzba)
        {
            profesori = new ObservableCollection<ShowProfessorsForStudentDTO>();
            this.student = s;
            this.ssluzba = ssluzba;

            InitializeComponent();
            PrikaziProfesore();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PrikaziProfesore()
        {
            profesori.Clear();
            foreach(Profesor p in ssluzba.GetProfessorsForStudent(student.id))
            {
                profesori.Add(new ShowProfessorsForStudentDTO(p));
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
