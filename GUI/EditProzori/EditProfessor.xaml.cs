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
    /// Interaction logic for EditProfessor.xaml
    /// </summary>
    public partial class EditProfessor : Window, IObserver
    {
        private StudentskaSluzbaDAO ssluzba;
        public ProfesorDTO profesor { get; set; }
        private int idSelected = -1;
        public ObservableCollection<PredmetDTO> Predmeti { get; set; }
        public EditProfessor(ProfesorDTO p,StudentskaSluzbaDAO sluzba,ObservableCollection<PredmetDTO> predmeti)
        {
            Predmeti=new ObservableCollection<PredmetDTO>();
            ssluzba = sluzba;
            profesor =new ProfesorDTO(p.ToProfesor(),sluzba);
            InitializeComponent();
            for (int i = 1; i <= 10; i++)
            {
                TextBox textBox = (TextBox)this.FindName("textBox" + i);
                if (textBox != null)
                {
                    textBox.LostFocus += TextBox_LostFocus;
                }
            }
            this.MouseDown += MainWindow_MouseDown;
            Add.IsEnabled = true;
            ssluzba.PredmetSubject.Subscribe(this);
            Closing += EditProfessor_Closing;
            Update();
        }
        private void EditProfessor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ssluzba.PredmetSubject.Unsubscribe(this);
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
        private void Close(object sender, RoutedEventArgs e)
        {
            ssluzba.PredmetSubject.Unsubscribe(this);
            Close();
        }
        private void Change(object sender, RoutedEventArgs e)
        {
            if (profesor.IsValid)
            {
                ssluzba.UpdateProfessor(profesor.ToProfesor());
                ssluzba.PredmetSubject.Unsubscribe(this);
                Close();
            }
            else
            {
                MessageBox.Show("Course can not be created. Not all fields are valid.");
            }
            
            /* int error = main.EditProfessor( ime,  prezime, datumRodjenja, adresaStanovanja, kontaktTelefon, emailAdresa, brojLicneKarte, zvanje, godineStaza,idKatedre);
             if (error == 1)
                 MessageBox.Show("Datum nije validan. Unesite datum u ovom formatu: dd.mm.yyyy. ");
             else if (error == 2)
                 MessageBox.Show("Adresa nije validna. Unesite adresu u ovom formatu: ulica broj grad, drzava");
             else if (error == 3)
                 MessageBox.Show("Katedra sa unetim Id-em ne postoji.");
             else
                 Close();*/
        }
        private void EnableAddButton()
        {
            bool allFieldsFilled = !string.IsNullOrEmpty(textBox1.Text) &&
                                    !string.IsNullOrEmpty(textBox2.Text) &&
                                    !string.IsNullOrEmpty(textBox5.Text) &&
                                    !string.IsNullOrEmpty(textBox6.Text) &&
                                    !string.IsNullOrEmpty(textBox3.Text) &&
                                     !string.IsNullOrEmpty(textBox7.Text) &&
                                      !string.IsNullOrEmpty(textBox8.Text) &&
                                       !string.IsNullOrEmpty(textBox9.Text) &&
                                        !string.IsNullOrEmpty(textBox10.Text) &&
                                    !string.IsNullOrEmpty(textBox4.Text);

            Add.IsEnabled = allFieldsFilled;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            EnableAddButton();
        }

        public void Update()
        {
            Predmeti.Clear();
            foreach (Predmet predmet in ssluzba._predmeti)
            {
                if (predmet.professorId == profesor.id)
                {
                    Predmeti.Add(new PredmetDTO(predmet));
                }
            }
        }

        private void AddPredmet(object sender, RoutedEventArgs e)
        {
            AddPredmetToProfessor noviProzor = new AddPredmetToProfessor(ssluzba,profesor.id);
            noviProzor.Owner = this;
            noviProzor.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            noviProzor.ShowDialog();
        }
        private void RemovePredmet(object sender, RoutedEventArgs e)
        {
            if (idSelected != -1)
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni?", "Ukloni predmet", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    //FUNKCIJA IMPLEMENTIRATI
                    ssluzba.RemovePredmetFromProfessor(profesor.id, idSelected);
                }
            }
        }

        private void DataGrid_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            foreach (PredmetDTO selectedItem in e.AddedItems)
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
