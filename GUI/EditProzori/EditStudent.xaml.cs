using CLI.Model;
using System;
using System.Collections.Generic;
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
using GUI.DTO;
using CLI.DAO;
using System.Diagnostics;
using System.Collections.ObjectModel;
using CLI.Observer;

namespace GUI
{
    /// <summary>
    /// Interaction logic for EditStudent.xaml
    /// </summary>
    public partial class EditStudent : Window, IObserver
    {
        private StudentskaSluzbaDAO ssluzba;
        int idSelected = -1;
        public StudentDTO student { get; set; }
        public ObservableCollection<PolozeniDTO> Polozeni { get; set; }
        public ObservableCollection<NePolozeniDTO> NePolozeni { get; set; }
        public EditStudent(StudentDTO s, StudentskaSluzbaDAO sluzba)
        {
            student = new StudentDTO(s.ToStudent(),sluzba);
            ssluzba = sluzba;
            DataContext = this;
            InitializeComponent();
            Polozeni = new ObservableCollection<PolozeniDTO>();
            NePolozeni = new ObservableCollection<NePolozeniDTO>();
            this.MouseDown += MainWindow_MouseDown;
            Add.IsEnabled = true;
            for (int i = 1; i <= 8; i++)
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
            ssluzba.OceneSubject.Subscribe(this);
            ssluzba.StudentPredmetSubject.Subscribe(this);
            ssluzba.StudentiSubject.Subscribe(this);
            Closing += EditStudent_Closing;
            Update();
        }
        private void EditStudent_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Debug.WriteLine("USAOdasdasdas");
            ssluzba.OceneSubject.Unsubscribe(this);
            ssluzba.StudentPredmetSubject.Unsubscribe(this);
            ssluzba.StudentiSubject.Unsubscribe(this);
        }
        public void Update()
        {
            Polozeni.Clear();
            int ukupnoespb = 0;
            double prosecnaocena = 0;
            int br = 0;
                foreach (Predmet predmet in ssluzba.GetStudentById(student.id).polozeniIspiti)//MOZDA PROBLEM KADA JE LISTA PRAZNA ILI NULL
                {
                    int ocena = 0;
                    string datum = "";
                    int idOcene = -1;
                    ukupnoespb = ukupnoespb + predmet.espb;
                    ssluzba.GetAllOcene().ForEach(o =>
                    {
                        if (o.idStudentPolozio == student.id && o.idPredmet == predmet.id)
                        {
                            ocena = o.ocena;
                            prosecnaocena = prosecnaocena + ocena;
                            br++;
                            datum = o.datumPolaganja.ToString();
                            Debug.WriteLine("GGGGGAAAAASSSSS " + ocena + " JASS " + datum);
                            idOcene = o.id;
                        }
                    });
                    Polozeni.Add(new PolozeniDTO(predmet, ocena, datum, idOcene));
                }

                dateTextBlock.Text = "Prosecna Ocena: " + prosecnaocena / br;
                dateTextBlock1.Text = "Ukupno ESPB: " + ukupnoespb;
            NePolozeni.Clear();
            foreach (Predmet predmet in ssluzba.GetStudentById(student.id).nepolozeniIspiti)//MOZDA PROBLEM KADA JE LISTA PRAZNA ILI NULL
            {
                Debug.WriteLine("-------TEST------" + predmet.naziv);
                NePolozeni.Add(new NePolozeniDTO(predmet));
            }
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
            ssluzba.OceneSubject.Unsubscribe(this);
            ssluzba.StudentPredmetSubject.Unsubscribe(this);
            ssluzba.StudentiSubject.Unsubscribe(this);
            Close();
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (student.IsValid)
            {
                ssluzba.UpdateStudent(student.ToStudent());

                ssluzba.OceneSubject.Unsubscribe(this);
                ssluzba.StudentPredmetSubject.Unsubscribe(this);
                ssluzba.StudentiSubject.Unsubscribe(this);
                Close();
            }
            else
            {
                MessageBox.Show("Course can not be created. Not all fields are valid.");
            }
           
            /* int error=main.EditStudent(prezime,ime,datumRodjena,adresaStanovanja,kontaktTelefon,emailAdresa,brojIndeksa,trenutnaGodinaStudija,status);
             if (error == 1)
                 MessageBox.Show("Datum nije validan. Unesite datum u ovom formatu: dd.mm.yyyy. ");
             else if (error == 2)
                 MessageBox.Show("Adresa nije validna. Unesite adresu u ovom formatu: ulica broj grad, drzava");
             else if (error == 3)
                 MessageBox.Show("Indeks nije validan. Unesite indeks u ovom formatu: XY brojIndeksa/godinaUpisa");
             else if (error == 4)
                 MessageBox.Show("Status nije validan. Unesite B ili S");
             else if (error == 5)
                 MessageBox.Show("Godina studija nije validna. Unesite godinu izmedju 1 i 4");
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
                                    !string.IsNullOrEmpty(textbox1.Text) &&
                                    !string.IsNullOrEmpty(textbox2.Text) ;

            Add.IsEnabled = allFieldsFilled;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            EnableAddButton();
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (PolozeniDTO selectedItem in e.AddedItems)
            {
                idSelected = selectedItem.idOcene;
                Debug.WriteLine("SELEKTOVAO SI :" + idSelected);
            }
        }
        private void DataGrid_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            foreach (NePolozeniDTO selectedItem in e.AddedItems)
            {
                idSelected = selectedItem.id;
                Debug.WriteLine("SELEKTOVAO SI :" + idSelected);
            }
        }
        private void DeleteOcenu(object sender, RoutedEventArgs e)
        {
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBoxResult.None;
            result = MessageBox.Show("Jeste li sigurni da želite da poništite ocenu", "Poništavanje ocene", buttons, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Debug.WriteLine("OCENA :" + idSelected);
                bool yes=ssluzba.RemoveOcena(idSelected);
                ssluzba.izracunajProsek(student.id);

            }
        }

        private void AddPredmet(object sender, RoutedEventArgs e)
        {
            AddPredmetToStudent noviProzor = new AddPredmetToStudent(ssluzba,student.id);
            noviProzor.Owner = this;
            noviProzor.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            noviProzor.ShowDialog();

        }

        private void RemovePredmet(object sender, RoutedEventArgs e)
        {
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBoxResult.None;
            result = MessageBox.Show("Jeste li sigurni da želite da uklonite predmet?" +
                "", "Uklanjanje predmeta ", buttons, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (ssluzba.RemoveStudentFromPredmet(student.id, idSelected))
                {
                    Debug.WriteLine("Obrisao");

                }
            }
        }

        private void DodajOcenu(object sender, RoutedEventArgs e)
        {
            
            foreach (NePolozeniDTO p in NePolozeni)
            {
                if(p.id == idSelected)
                {
                    Polaganje prozor = new Polaganje(p, student.id, ssluzba);
                    prozor.Owner = this;
                    prozor.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    prozor.ShowDialog();
                    break;
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
