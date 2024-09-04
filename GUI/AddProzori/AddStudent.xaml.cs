using CLI.DAO;
using CLI.Model;
using GUI.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.DirectoryServices;
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
    /// Interaction logic for AddStudent.xaml
    /// </summary>
    public partial class AddStudent : Window, INotifyPropertyChanged
    {
       /* private bool proveraDatuma = true;
        private bool proveraIndeksa = true;
        private bool proveraAdrese = true;

        private string datumRodjena;
        public string DatumRodjena
        {
            get
            {
                return datumRodjena;
            }
            set
            {
                if (value != datumRodjena)
                {
                    if (value != null && ssluzba.DatumRegex(value))
                    {
                        Student.Datum = value;
                        proveraDatuma = true;
                    }
                    else
                    {
                        proveraDatuma = false;
                        MessageBox.Show("Datum nije validan. Unesite datum u ovom formatu: dd.mm.yyyy. ");
                    }
                    datumRodjena = value;
                    OnPropertyChanged("DatumRodjena");
                }
            }
        }
        private string adresaStanovanja;
        public string AdresaStanovanja
        {
            get
            {
                return adresaStanovanja;
            }
            set
            {
                if (value != adresaStanovanja)
                {
                    if (value != null && ssluzba.AdresaRegex(value))
                    {
                        Student.Adresa = value;
                        proveraAdrese = true;
                    }
                    else
                    {
                        proveraAdrese = false;
                        MessageBox.Show("Adresa nije validna. Unesite adresu u ovom formatu: ulica broj grad, drzava");
                    }
                    adresaStanovanja = value;
                    OnPropertyChanged("AdresaStanovanja");
                }
            }
        }
        private string brojIndeksa;
        public string BrojIndeksa
        {
            get
            {
                return brojIndeksa;
            }
            set
            {
                if (value != brojIndeksa)
                {
                    if (value != null && ssluzba.IndexRegex(value))
                    {
                        Student.Indeks = value;
                        proveraIndeksa = true;
                    }
                    else
                    {
                        proveraIndeksa = false;
                        MessageBox.Show("Indeks nije validan. Unesite indeks u ovom formatu: XY brojIndeksa/godinaUpisa");
                    }
                    brojIndeksa = value;
                    OnPropertyChanged("BrojIndeksa");
                }
            }
        }
        public string trenutnaGodinaStudija;
        public string TrenutnaGdoinaStudija
        {
            get
            {
                return trenutnaGodinaStudija;
            }
            set
            {
                if (value != trenutnaGodinaStudija)
                {
                    GodinaStudija();
                    Student.TrenutnaGodinaStudija = trenutnaGodinaStudija;
                    OnPropertyChanged("TrenutnaGodinaStudija");
                }
            }
        }
        private string status;
        public string Status { 
            get
            {
                return status;
            }
            set
            {
                if (value != status)
                {
                    StatusStudenta();
                    status= value;
                    OnPropertyChanged("Status");
                }
            }
        }*/
        public event PropertyChangedEventHandler? PropertyChanged;
        private StudentskaSluzbaDAO ssluzba;
        public StudentDTO Student { get; set; }
        public AddStudent(StudentskaSluzbaDAO sluzba)
        {
            ssluzba= sluzba;
            Student = new StudentDTO(sluzba);
            InitializeComponent();
            Add.IsEnabled = false;
            for (int i = 1; i <= 8; i++)
            {
                TextBox textBox = (TextBox)this.FindName("textBox" + i);
                if (textBox != null)
                {
                    textBox.LostFocus += TextBox_LostFocus;
                }

            }
            this.MouseDown += MainWindow_MouseDown;
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
            if (Student.IsValid)
            {
                ssluzba.AddStudent(Student.ToStudent());
                Close();
            }
            else
            {
                MessageBox.Show("Course can not be created. Not all fields are valid.");
            }
           
        }


        /*private void GodinaStudija()
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    trenutnaGodinaStudija = "1";
                    break;
                case 1:
                    trenutnaGodinaStudija = "2";
                    break;
                case 2:
                    trenutnaGodinaStudija = "3";
                    break;
                case 3:
                    trenutnaGodinaStudija = "4";
                    break;
            }
        }

        private void StatusStudenta()
        {
            if (comboBox2.SelectedIndex == 0)
            {
                Student.Status = CLI.Model.Status.S;
            }
            else
            {
                Student.Status = CLI.Model.Status.B;
            }
                
        }*/

        private void EnableAddButton()
        {
            bool allFieldsFilled = !string.IsNullOrEmpty(textBox1.Text) &&
                                    !string.IsNullOrEmpty(textBox2.Text) &&
                                    !string.IsNullOrEmpty(textBox3.Text) &&
                                    !string.IsNullOrEmpty(textBox4.Text) &&
                                    !string.IsNullOrEmpty(textBox5.Text) &&
                                    !string.IsNullOrEmpty(textBox6.Text) &&
                                    !string.IsNullOrEmpty(textBox7.Text) &&
                                    !string.IsNullOrEmpty(textBox8.Text);
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
