using CLI.DAO;
using CLI.Model;
using GUI.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for AddPredmet.xaml
    /// </summary>
    public partial class AddPredmet : Window
    {
       /* private string semestar;
        public string Semestar
        {
            get
            {
                return semestar;
            }
            set
            {
                if (value != semestar)
                {
                    SemestarPredmeta();
                    OnPropertyChanged("Semestar");
                }
            }
        }
        public string godinaSlusanja;
        public string GodinaSlusanja
        {
            get
            {
                return godinaSlusanja;
            }
            set
            {
                if (value != godinaSlusanja)
                {
                    GodinaSlusanjaPredmeta();
                    OnPropertyChanged("GodinaSlusanja");
                }
            }
        }
        private bool provera = true;
        public int? professorId;
        public int? ProfessorId
        {
            get
            {
                return professorId;
            }
            set
            {
                if (value != professorId)
                {
                    if(value == -1)
                    {
                        professorId = value;
                        provera = true;
                        Predmet.IdProf = (int)professorId;
                    }
                    else if(ssluzba.GetAllProfessors().FindIndex(p => p.id == value) == -1)
                    {
                        MessageBox.Show("Profesor sa tim ID-em ne postoji");
                        provera = false;
                        professorId = value;
                    }
                    else
                    {
                        provera = true;
                        professorId = value;
                        Predmet.IdProf =(int)professorId;
                    }
                    OnPropertyChanged("ProfessorId");
                }
            }
        }*/


        public event PropertyChangedEventHandler? PropertyChanged;
        private StudentskaSluzbaDAO ssluzba;
        public PredmetDTO Predmet { get; set; }
        public AddPredmet(StudentskaSluzbaDAO sluzba)
        {
            ssluzba = sluzba;
            Predmet = new PredmetDTO(ssluzba);
            InitializeComponent();
            for (int i = 1; i <= 6; i++)
            {
                TextBox textBox = (TextBox)this.FindName("textBox" + i);
                if (textBox != null)
                {
                    textBox.LostFocus += TextBox_LostFocus;
                }
            }
            this.MouseDown += MainWindow_MouseDown;
            Add.IsEnabled = false;

        }
        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UIElement clickedElement = e.OriginalSource as UIElement;

            if (clickedElement != null)
            {
                clickedElement.Focusable = true;
                clickedElement.Focus();
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ADD(object sender, RoutedEventArgs e)
        {
            if (Predmet.IsValid)
            {
                ssluzba.AddPredmet(Predmet.ToPredmet());
                Close();
            }
            else
            {
                MessageBox.Show("Course can not be created. Not all fields are valid.");
            }
           
            /*int error=mainWindow.AddPredmet(sifra, naziv, semestar, godinaStudija, professorId, espb);
            if (error == 1)
                MessageBox.Show("Profesor sa tim ID-em ne postoji");
            else
                Close();*/
        }


        /*private void GodinaSlusanjaPredmeta()
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    godinaSlusanja = "1";
                    Predmet.GodinaStudija = 1;
                    break;
                case 1:
                    godinaSlusanja = "2";
                    Predmet.GodinaStudija = 2;
                    break;
                case 2:
                    godinaSlusanja = "3";
                    Predmet.GodinaStudija = 3;
                    break;
                case 3:
                    godinaSlusanja ="4";
                    Predmet.GodinaStudija = 4;
                    break;
            }

            Debug.WriteLine(godinaSlusanja);
        }*/

        /*private void SemestarPredmeta()
        {
            if (comboBox1.SelectedIndex == 0)
            {
                semestar = "Letnji";
                Predmet.Semestar = CLI.Model.Semestar.Letnji;
            }
            else
            {
                semestar = "Zimski";
                Predmet.Semestar = CLI.Model.Semestar.Zimski;
            }
            Debug.WriteLine(semestar);
        }*/

       

        private void textBox5_LostFocus(object sender, RoutedEventArgs e)
        {

        }

       
        private void EnableAddButton()
        {
            bool allFieldsFilled = !string.IsNullOrEmpty(textBox1.Text) &&
                                    !string.IsNullOrEmpty(textBox2.Text) &&
                                    !string.IsNullOrEmpty(textBox5.Text) &&
                                    !string.IsNullOrEmpty(textBox6.Text);
                                    

            Add.IsEnabled = allFieldsFilled;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            EnableAddButton();
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
