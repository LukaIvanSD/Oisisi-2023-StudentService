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
    /// Interaction logic for EditPredmet.xaml
    /// </summary>
    public partial class EditPredmet : Window
    {
       /* bool provera = true;
        bool provera1 = true;
        bool provera2 = true;
        /*private int godinastudija;
        public int GodinaStudija
        {
            get
            {
                return godinastudija;
            }
            set
            {
                if (value != godinastudija)
                {
                    if (value > 4 || value < 1)
                    {
                        MessageBox.Show("Godina studija nije validna. Unesite godinu studija izmedju 1 i 4");
                         provera1 = false;
                        godinastudija = value;
                    }
                    else
                    {
                         provera1 = true;
                        godinastudija = value;
                        predmet.GodinaStudija = godinastudija;
                        OnPropertyChanged("GodinaStudija");
                    }    
                }
            }
        }
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
                    if (ssluzba.GetAllProfessors().FindIndex(p => p.id == value) == -1)
                    {
                        MessageBox.Show("Profesor sa tim ID-em ne postoji");
                        provera = false;
                        professorId = value;
                    }
                    else
                    {
                        provera = true;
                        professorId = value;
                        predmet.IdProf = (int)professorId;
                    }
                    OnPropertyChanged("ProfessorId");
                }
            }
        }
        private string semestar;
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
                    if (value.ToLower() == "letnji")
                    {
                        provera2=true;
                        predmet.Semestar = CLI.Model.Semestar.Letnji;
                        semestar = value;
                        OnPropertyChanged("Semestar");
                    }
                    else if (value.ToLower() == "zimski")
                    {   provera2 =true;
                        predmet.Semestar = CLI.Model.Semestar.Zimski;
                        semestar = value;
                        OnPropertyChanged("Semestar");
                    }
                    else
                    { 
                        provera2 =false;
                        MessageBox.Show("Semestar nije validan. Unesite ili Letnji ili Zimski semestar");
                    }
                       
                }
            }
        }*/
        public event PropertyChangedEventHandler? PropertyChanged;

        public PredmetDTO predmet { get; set; }
        private StudentskaSluzbaDAO ssluzba;
        public EditPredmet(PredmetDTO p,StudentskaSluzbaDAO sluzba)
        {
            predmet = new PredmetDTO(p.ToPredmet(),sluzba);
            ssluzba = sluzba;
           /* GodinaStudija = p.GodinaStudija;
            Semestar = p.Semestar.ToString();
            ProfessorId = p.IdProf;*/
            InitializeComponent();

            if (predmet.IdProf == -1)
            {
                DodajProfu.IsEnabled = true;
                UkloniProfu.IsEnabled = false;
            }
            else
            {
                DodajProfu.IsEnabled = false;
                UkloniProfu.IsEnabled = true;
            }

            for (int i = 1; i <= 6; i++)
            {
                TextBox textBox = (TextBox)this.FindName("textBox" + i);
                if (textBox != null)
                {
                    textBox.LostFocus += TextBox_LostFocus;
                }
            }
            for (int i = 1; i <= 2; i++)
            {
                TextBox textBox = (TextBox)this.FindName("textbox" + i);
                if (textBox != null)
                {
                    textBox.LostFocus += TextBox_LostFocus;
                }
            }
            this.MouseDown += MainWindow_MouseDown;
            Add.IsEnabled = true;
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

        private void Change(object sender, RoutedEventArgs e)
        {
            if (predmet.IsValid)
            {
                ssluzba.UpdatePredmet(predmet.ToPredmet());
                Close();
            }
            else
            {
                MessageBox.Show("Course can not be created. Not all fields are valid.");
            }
           
        }
        private void EnableAddButton()
        {
            bool allFieldsFilled = !string.IsNullOrEmpty(textBox1.Text) &&
                                    !string.IsNullOrEmpty(textBox2.Text) &&
                                    !string.IsNullOrEmpty(textBox5.Text) &&
                                    !string.IsNullOrEmpty(textBox6.Text) &&
                                    !string.IsNullOrEmpty(textbox2.Text) &&
                                    !string.IsNullOrEmpty(textbox1.Text);

            Add.IsEnabled = allFieldsFilled;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            EnableAddButton();
        }

        private void AddProfessor(object sender, RoutedEventArgs e)
        {
            int before = predmet.IdProf;
            AddProfessorToPredmet noviProzor = new AddProfessorToPredmet(ssluzba,predmet.IdProf, predmet);
            noviProzor.Owner = this;
            noviProzor.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            noviProzor.ShowDialog();
            if (before != predmet.IdProf)
            {
                textBox5.Text = predmet.IdProf.ToString();//OVO JE MALO GLUPO ODRADJENO
                DodajProfu.IsEnabled = false;
                UkloniProfu.IsEnabled = true;
            }
        }

        private void RemoveProfessor(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni?", "Ukloni profesora", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                
                //FUNKCIJA IMPLEMENTIRATI
                if (ssluzba.RemovePredmetFromProfessor(predmet.IdProf, predmet.id))
                {
                    textBox5.Text = "-1";
                    DodajProfu.IsEnabled = true;
                    UkloniProfu.IsEnabled = false;
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
