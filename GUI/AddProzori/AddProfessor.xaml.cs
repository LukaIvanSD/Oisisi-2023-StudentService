using CLI.DAO;
using CLI.Model;
using GUI.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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
    /// Interaction logic for AddProfessor.xaml
    /// </summary>
    public partial class AddProfessor : Window
    {
       /* bool proveraDatuma = true;
        private bool provera = true;
        private bool proveraAdrese = true;
        private string datumRodjenja;
        public string DatumRodjenja
        {
            get
            {
                return datumRodjenja;
            }
            set
            {
                if (value != datumRodjenja)
                {
                    if (value != null && ssluzba.DatumRegex(value))
                    {
                        Profesor.DatumRodjenja = value;
                        proveraDatuma = true;
                    }
                    else
                    {
                        proveraDatuma = false;
                        MessageBox.Show("Datum nije validan. Unesite datum u ovom formatu: dd.mm.yyyy. ");
                    }
                    datumRodjenja = value;
                    OnPropertyChanged("DatumRodjenja");
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
                        Profesor.AdresaStanovanja = value;
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
        private int? idKatedre;
        public int? IdKatedre
        {
            get
            {
                return idKatedre;
            }
            set
            {
                if (value != idKatedre)
                {
                    if (ssluzba.GetAllKatedre().FindIndex(k => k.id == value) == -1)
                    {
                        MessageBox.Show("Katedra sa unetim ID ne postoji, pokusajte ponovo.");
                        provera = false;
                        idKatedre = value;
                    }
                    else
                    {
                        provera = true;
                        idKatedre = value;
                        Profesor.IdKatedre = (int)idKatedre;
                    }
                    OnPropertyChanged("IdKatedre");
                }
            }
        }*/
        public event PropertyChangedEventHandler? PropertyChanged;
        private StudentskaSluzbaDAO ssluzba;
        public ProfesorDTO Profesor { get; set; }
        public AddProfessor(StudentskaSluzbaDAO sluzba)
        {
            ssluzba = sluzba;
            Profesor=new ProfesorDTO(sluzba);
            InitializeComponent();
            for (int i = 1; i <= 10; i++)
            {
                TextBox textBox = (TextBox)this.FindName("textBox" + i);
                if (textBox != null)
                {
                    textBox.LostFocus += TextBox_LostFocus;
                }

            }
            Add.IsEnabled= false;
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
            if (Profesor.IsValid)
            {
                ssluzba.AddProfesor(Profesor.ToProfesor());
                Close();
            }
            else
            {
                MessageBox.Show("Professor can not be created. Not all fields are valid.");
            }
            
            /* int error = mainWindow.AddProfessor(ime, prezime, datumRodjenja, adresaStanovanja, kontaktTelefon, emailAdresa,brojLicneKarte,zvanje,godineStaza,idKatedre);
             if (error == 1)
                 MessageBox.Show("Datum nije validan. Unesite datum u ovom formatu: dd.mm.yyyy. ");
             else if(error==2)
                 MessageBox.Show("Adresa nije validna. Unesite adresu u ovom formatu: ulica broj grad, drzava");
             else if(error==3)
                 MessageBox.Show("Katedra sa unetim Id-em ne postoji.");
             else
             Close();*/
        }
        private void EnableAddButton()
        {
            bool allFieldsFilled = !string.IsNullOrEmpty(textBox1.Text) &&
                                    !string.IsNullOrEmpty(textBox2.Text) &&
                                    !string.IsNullOrEmpty(textBox3.Text) &&
                                    !string.IsNullOrEmpty(textBox4.Text) &&
                                    !string.IsNullOrEmpty(textBox5.Text) &&
                                    !string.IsNullOrEmpty(textBox6.Text) &&
                                    !string.IsNullOrEmpty(textBox7.Text) &&
                                    !string.IsNullOrEmpty(textBox8.Text) &&
                                    !string.IsNullOrEmpty(textBox9.Text) &&
                                    !string.IsNullOrEmpty(textBox10.Text);

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
