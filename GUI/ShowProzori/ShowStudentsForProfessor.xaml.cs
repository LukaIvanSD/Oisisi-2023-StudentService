using CLI.DAO;
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
    /// Interaction logic for ShowStudentsForProfessor.xaml
    /// </summary>
    public partial class ShowStudentsForProfessor : Window
    {

        private StudentskaSluzbaDAO ssluzba;
        public ProfesorDTO profesor { get; set; }

        public ObservableCollection<ShowStudentsForProfessorDTO> studenti { get; set; }
        public ShowStudentsForProfessor(ProfesorDTO p, StudentskaSluzbaDAO s)
        {
            studenti = new ObservableCollection<ShowStudentsForProfessorDTO>();
            this.ssluzba = s;
            this.profesor = p;
            InitializeComponent();
            PrikaziStudenteZaProfesora();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PrikaziStudenteZaProfesora()
        {
            studenti.Clear();
            foreach(CLI.Model.Student s in ssluzba.GetStudentsForProfessor(profesor.id))
            {
                studenti.Add(new ShowStudentsForProfessorDTO(s));
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
