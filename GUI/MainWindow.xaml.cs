using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using CLI.DAO;
using CLI.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using CLI.Observer;
using GUI.DTO;
using System.Collections;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IObserver
    {
        private static App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";

        private DispatcherTimer timer;
        private int currentTabIndex = -1; // Inicijalno postavite na 0 jer se na pocetku nalazi na studentu
        int idSelected = -1;
        private string search;
        const int elementsPerPage = 16;
        int stranica = 0;
        public ObservableCollection<StudentDTO> Studenti { get; set; }
        public StudentDTO SelectedStudent { get; set; }
        private StudentskaSluzbaDAO ssluzba { get; set; }
        private List<PredmetDTO> selectedPredmets { get; set; }
        public ObservableCollection<ProfesorDTO> Profesori { get; set; }
        public ProfesorDTO SelectedProfesor { get; set; }
        public ObservableCollection<PredmetDTO> Predmeti { get; set; }
        public PredmetDTO SelectedPredmet { get; set; }
        public ObservableCollection<KatedraDTO> Katedre { get; set; }
        public KatedraDTO SelectedKatedra { get; set; }
        private List<Predmet> foundCourses;
        private List<Student> foundStudents;
        private List<Profesor> foundProfessors;
        private List<Katedra> foundDepartments;
        public MainWindow()
        {
            selectedPredmets = new List<PredmetDTO>();
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            DataContext = this;
            Studenti = new ObservableCollection<StudentDTO>();
            Profesori = new ObservableCollection<ProfesorDTO>();
            Predmeti = new ObservableCollection<PredmetDTO>();
            Katedre = new ObservableCollection<KatedraDTO>();
            foundCourses = new List<Predmet>();
            foundStudents=new List<Student>();
            foundDepartments=new List<Katedra>();
            foundProfessors=new List<Profesor>();
            ssluzba = new StudentskaSluzbaDAO();
            ssluzba.StudentiSubject.Subscribe(this);
            ssluzba.ProfesoriSubject.Subscribe(this);
            ssluzba.PredmetSubject.Subscribe(this);
            ssluzba.KatedreSubject.Subscribe(this);
            Update();
            app = (App)Application.Current;
            app.ChangeLanguage(SRB);
            Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;
            Top = (SystemParameters.PrimaryScreenHeight - Height) / 2;
            Closing += MainWindow_Closing;
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li zaista želite zatvoriti prozor?", "Potvrda zatvaranja", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else {
                ssluzba.SaveWhenExit();
            }
        }
        public void Update()
        {
            searchbox.Text = "";
            search = "";
            Studenti.Clear();
            int iteracija = 0;
            foreach (Student student in ssluzba.GetAllStudents())
            {
                if (elementsPerPage * stranica <= iteracija)
                    Studenti.Add(new StudentDTO(student));
                if (Studenti.Count == elementsPerPage)
                    break;
                iteracija++;
            }
            Profesori.Clear();
            iteracija = 0;
            foreach (Profesor profesor in ssluzba.GetAllProfessors())
            {
                if (elementsPerPage * stranica <= iteracija)
                    Profesori.Add(new ProfesorDTO(profesor));
                if (Profesori.Count == elementsPerPage)
                    break;
                iteracija++;
            }
            Predmeti.Clear();
            iteracija = 0;
            foreach (Predmet predmet in ssluzba.GetAllPredmeti())
            {
                if (elementsPerPage * stranica <= iteracija)
                    Predmeti.Add(new PredmetDTO(predmet));
                if (Predmeti.Count == elementsPerPage)
                    break;
                iteracija++;
                Debug.WriteLine("Predmet: " + predmet.ToString());
            }
            Katedre.Clear();
            iteracija = 0;
            foreach (Katedra katedra in ssluzba.GetAllKatedre())
            {
                if (elementsPerPage * stranica <= iteracija)
                    Katedre.Add(new KatedraDTO(katedra));
                if (Katedre.Count == elementsPerPage)
                    break;
                iteracija++;
            }
        }
        //------------------------EVENT HANDLERI---------------------------------------

        private void MenuItem_Click_Serbian(object sender, RoutedEventArgs e)
        {
            ChangeToSerbian();
        }

        private void MenuItem_Click_English(object sender, RoutedEventArgs e)
        {
            ChangeToEnglish();
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (StudentDTO selectedItem in e.AddedItems)
            {
                idSelected = selectedItem.id;
                Debug.WriteLine("SELEKTOVAO SI :" + idSelected);
            }
        }
        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            foreach (ProfesorDTO selectedItem in e.AddedItems)
            {
                idSelected = selectedItem.id;
                Debug.WriteLine("SELEKTOVAO SI :" + idSelected);
            }
        }
        private void DataGrid_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
          
            foreach (PredmetDTO selectedItem in e.AddedItems)
            {
                idSelected = selectedItem.id;
                selectedPredmets.Add(selectedItem);
                Debug.WriteLine("SELEKTOVAO SI :" + idSelected);
            }
            int selectedRowCount = dataPredmeti.SelectedItems.Count;
            Debug.WriteLine("BROJ LINIJA:"+selectedRowCount);
            if (selectedRowCount == 2)
            {
                foreach (PredmetDTO p in selectedPredmets)
                {
                    Debug.WriteLine("AD:" + p.Naziv);
                }
                PrikaziEntitete.IsEnabled = true;
            }
            else if (selectedRowCount == 1)
            {
                PrikaziEntitete.IsEnabled = false;
            }
            else
            {//Sada valjda radi
                if (selectedPredmets.Count > 1)
                { selectedPredmets.RemoveRange(0, selectedPredmets.Count-1); }

            }
        }
        private void DataGrid_SelectionChanged_3(object sender, SelectionChangedEventArgs e)
        {
            foreach (KatedraDTO selectedItem in e.AddedItems)
            {
                idSelected = selectedItem.id;
                Debug.WriteLine("SELEKTOVAO SI :" + idSelected);
            }
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string status = "Studentska služba - ";

            if (currentTabIndex != MainTabControl.SelectedIndex)
            {
                idSelected = -1; // DA NE BI BRISAO PROFU 0 KAD SE PROMENI SA STUDENTA 0 NPR
                selectedPredmets.Clear();
                PrikaziEntitete.IsEnabled = true;
                currentTabIndex = MainTabControl.SelectedIndex;

                switch (currentTabIndex)
                {
                    case 0:
                        stranica = 0;
                        Update();//DA LI OVO SADA TREBA????????
                        searchbox.Text = "";
                        search = "";
                        statusbar.Content = status + "Studenti";
                        break;
                    case 1:
                        stranica = 0;
                        Update();//DA LI OVO SADA TREBA????????                       
                        searchbox.Text = "";
                        search = "";
                        statusbar.Content = status + "Profesori";
                        break;
                    case 2:
                        stranica = 0;
                        Update();//DA LI OVO SADA TREBA????????
                        searchbox.Text = "";
                        search = "";
                        statusbar.Content = status + "Predmeti";
                        break;
                    case 3:
                        stranica = 0;
                        Update();//DA LI OVO SADA TREBA????????
                        searchbox.Text = "";
                        search = "";
                        statusbar.Content = status + "Katedre";
                        break;
                }
            }

        }
        private void AddEntityButton_Click(object sender, RoutedEventArgs e)
        {
            OpenAddEntityWindow();
        }
        private void CloseApp(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            dateTextBlock.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
        }
        private void Open(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }
        private void New(object sender, RoutedEventArgs e)
        {
            NewEntity();
        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            DeleteEntityWindow();
        }

        private void ShowAll(object sender, RoutedEventArgs e)
        {
            ShowItem();
        }
        private void About(object sender, RoutedEventArgs e)
        {
            AboutUs();
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }
        private void Edit(object sender, RoutedEventArgs e)
        {
            EditItem();
        }
        private void Search(object sender, RoutedEventArgs e)
        {
            search = searchbox.Text;
            SearchEntity();
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.S)
            { e.Handled = true; SaveFile(); }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.B)
            { e.Handled = true; ChangeToEnglish(); }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.R)
            { e.Handled = true; ChangeToSerbian(); }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.O)
            { e.Handled = true; OpenFile(); }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.N)
            { e.Handled = true; NewEntity(); }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.C)
            { e.Handled = true; Close(); }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.D)
            { e.Handled = true; DeleteEntityWindow(); }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.E)
            { e.Handled = true; EditItem(); }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.A)
            { e.Handled = true; AboutUs(); }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.P)
            { e.Handled = true; search = searchbox.Text; SearchEntity(); }
        }
        //------------------------------------------------------------------------------------------
        //-------------------------DOMACE FUNKCIJE---------------------------------------------------
        private void Close()
        {
            ssluzba.SaveWhenExit();
            Application.Current.Shutdown();
        }
        private void OpenFile()
        {
        }
        private void SaveFile()
        {
        }
        public static void ChangeToSerbian()
        {
            app.ChangeLanguage(SRB);
        }
        public static void ChangeToEnglish()
        {
            app.ChangeLanguage(ENG);
        }
        private void NewEntity()
        {
            OpenAddEntityWindow();
        }
        private void EditItem()
        {
            EditEntityWindow();
        }
        private void ShowItem()
        {
            ShowAllEntitiesWindow();
        }
        private void AboutUs()
        {
            About noviProzor = new About();
            noviProzor.Owner = this;
            noviProzor.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            noviProzor.Show();
        }
        //---------------------------------------------------------------------------------

        //-------------------------------Logika za dodavanje entiteta---------------------
        //SVAKA FUNKCIJA VRACA INT KAO INDEKS USPESNOSTI DODAVANJA 0 OZNACAVA USPOSNO DODAVANJA A SVE OSTALO NEUSPESNO
        private void OpenAddEntityWindow()
        {
            switch (currentTabIndex)
            {
                case 0:
                    OpenAddStudentWindow();
                    break;
                case 1:
                    OpenAddProfessorWindow();
                    break;
                case 2:
                    OpenAddCourseWindow();
                    break;
                case 3:
                    OpenAddKatedraWindow();
                    break;
                default:
                    break;
            }
        }

        private void OpenAddStudentWindow()
        {
            AddStudent addProzor = new AddStudent(ssluzba);
            addProzor.Owner = this;
            addProzor.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addProzor.ShowDialog();
        }

        private void OpenAddProfessorWindow()
        {
            AddProfessor addProzor = new AddProfessor(ssluzba);
            addProzor.Owner = this;
            addProzor.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addProzor.ShowDialog();
        }

        private void OpenAddCourseWindow()
        {
            AddPredmet addProzor = new AddPredmet(ssluzba);
            addProzor.Owner = this;
            addProzor.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addProzor.ShowDialog();
        }
        private void OpenAddKatedraWindow()
        {
            AddKatedra addProzor = new AddKatedra(ssluzba);
            addProzor.Owner = this;
            addProzor.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addProzor.ShowDialog();
        }

        //------------------------------------------------------------------------------

        //-------------------------------Logika za brisanje entiteta---------------------
        private void DeleteEntityWindow()
        {
            switch (currentTabIndex)
            {
                case 0:
                    Delete(0);
                    break;
                case 1:
                    Delete(1);
                    break;
                case 2:
                    Delete(2);
                    break;
                case 3:
                    Delete(3);
                    break;
                default:
                    break;
            }
        }
        private void Delete(int index)
        {
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBoxResult.None;
            switch (index)
            {
                case 0:
                    result = MessageBox.Show("Jeste li sigurni da želite izbrisete studenta?", "Brisanje studenta", buttons, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        ssluzba.RemoveStudent(idSelected);
                    }
                    break;
                case 1:
                    bool provera = true;
                    result = MessageBox.Show("Jeste li sigurni da želite izbrisete profesora?", "Brisanje profesora", buttons, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        ssluzba._profesori.ForEach(p => {
                            if (p.id == idSelected && p.predaje.Count != 0)
                            {
                                provera = false;
                            }
                        });
                        if (provera == true)
                        {
                            ssluzba.RemoveProfesor(idSelected);
                        }
                        else
                            MessageBox.Show("Ne moze se obrisati profesor, jer nisu uklonjeni svi predmeti koje predaje");
                    }
                    break;
                case 2:
                    bool provera1 = true;
                    result = MessageBox.Show("Jeste li sigurni da želite izbrisete predmet?", "Brisanje premdeta", buttons, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        ssluzba._predmeti.ForEach(p => {
                            if (p.id == idSelected && p.nisuPoloziliPredmet.Count != 0)
                            {
                                provera1 = false;
                            }
                        });
                        if (provera1 == true)
                        {
                            ssluzba.RemovePredmet(idSelected);
                        }
                        else
                            MessageBox.Show("Nije moguce izbrisati predmet jer ga nisu svi studenti polozili. ");
                    }
                    break;
                case 3:
                    bool provera2 = true;
                    result = MessageBox.Show("Jeste li sigurni da želite izbrisete katedru?", "Brisanje studenta", buttons, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        ssluzba._katedre.ForEach(k =>
                        {
                            if (k.id == idSelected && k.profesori.Count != 0)
                            {
                                provera2 = false;
                            }
                        });
                        if (provera2 == true)
                            ssluzba.RemoveKatedra(idSelected);
                        else
                            MessageBox.Show("Ne moze se obrisati katedra, jer nisu uklonjeni svi profesori iz nje");
                    }
                    break;
                default:
                    break;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)//OOV MORA DA SE PROVERI GDE SE ZOVE
        {
            MainTabControl.SelectedIndex = 0;
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 2;
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 1;
        }
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 3;
        }




        //------------------------------------------------------------------------------

        //-------------------------------Logika za Izmenu entiteta---------------------
        private void EditEntityWindow()
        {

            switch (currentTabIndex)
            {
                case 0:
                    if (SelectedStudent != null)
                    {
                        EditStudent noviProzor = new EditStudent(SelectedStudent, ssluzba);//OVDE SE NE DESAVA BAS NAJBOLJE PRILIKO IZMENE
                        noviProzor.Owner = this;
                        noviProzor.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        noviProzor.ShowDialog();
                    }
                    else
                        MessageBox.Show("Greska nije selektovan ni jedan student");
                    break;
                case 1:
                    if (SelectedProfesor != null)
                    {
                        EditProfessor noviProzor2 = new EditProfessor(SelectedProfesor, ssluzba, Predmeti);
                        noviProzor2.Owner = this;
                        noviProzor2.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        noviProzor2.ShowDialog();
                    }
                    else
                        MessageBox.Show("Greska nije selektovan ni jedan profesor");
                    break;
                case 2:
                    if (SelectedPredmet != null)
                    {
                        EditPredmet noviProzor1 = new EditPredmet(SelectedPredmet, ssluzba);
                        noviProzor1.Owner = this;
                        noviProzor1.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        noviProzor1.ShowDialog();
                    }
                    else
                        MessageBox.Show("Greska nije selektovan ni jedan predmet");
                    break;
                case 3:
                    if (SelectedKatedra != null)
                    {
                        EditKatedra noviProzor = new EditKatedra(SelectedKatedra, ssluzba);
                        noviProzor.Owner = this;
                        noviProzor.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        noviProzor.ShowDialog();
                    }
                    else
                        MessageBox.Show("Greska nije selektovana ni jedana katedra");
                    break;
                default:
                    break;
            }
        }

        private void ShowAllEntitiesWindow()
        {
            switch (currentTabIndex)
            {
                case 0:
                    if (SelectedStudent != null)
                    {
                        ShowProfessorsForStudent noviProzor = new ShowProfessorsForStudent(SelectedStudent, ssluzba);
                        noviProzor.Owner = this;
                        noviProzor.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        noviProzor.ShowDialog();
                    }
                    else
                        MessageBox.Show("Greska nije selektovan ni jedan student");
                    break;
                case 1:
                    if (SelectedProfesor != null)
                    {
                        ShowStudentsForProfessor noviProzor = new ShowStudentsForProfessor(SelectedProfesor, ssluzba);
                        noviProzor.Owner = this;
                        noviProzor.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        noviProzor.ShowDialog();
                    }
                    else
                        MessageBox.Show("Greska nije selektovan ni jedan profesor");
                    break;
                case 2:
                    if (selectedPredmets.Count == 2)
                    { 
                        ShowStudentsForPredmet noviProzor = new ShowStudentsForPredmet(selectedPredmets, ssluzba);
                    noviProzor.Owner = this;
                    noviProzor.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    noviProzor.ShowDialog();
            }
                    break;
                case 3:
                    Debug.WriteLine("gAS");
                    if (SelectedKatedra != null)
                    {
                        Debug.WriteLine("gasdsadsAS");
                        ShowPredmetiForKatedra noviProzor = new ShowPredmetiForKatedra(SelectedKatedra, ssluzba);
                        noviProzor.Owner = this;
                        noviProzor.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        noviProzor.ShowDialog();
                    }
                    else
                        MessageBox.Show("Greska, nije selektovana ni jedna katedra");
                    break;
                default:
                    break;
            }
        }
        /* public int EditStudent(string prezime,string ime,string datumRodjenja,string adresaStanovanja,string kontaktTelefon,
                 string emailAdresa,string brojIndeksa,string trenutnaGodinaStudija,string status)
         {
             Datum datum = new Datum();
             if (!ssluzba.DatumRegex(datumRodjenja))
             {
                 return 1;
             }
             datum.setDatum(datumRodjenja);
             Adresa adresa = new Adresa();
             if (!ssluzba.AdresaRegex(adresaStanovanja))
             {
                 return 2;
             }
             adresa.setAdresa(adresaStanovanja);
             Indeks indeks = new Indeks();
             if (!ssluzba.IndexRegex(brojIndeksa))
             {
                 return 3;
             }
             indeks.SetIndex(brojIndeksa);

             Status status1;
             if (status.ToLower() == "budžet" || status.ToLower() == "b")
                 status1 = Status.B;
             else if (status.ToLower() == "samofinansirajući" || status.ToLower() == "s")
                 status1 = Status.S;
             else
                 return 4;
             if (trenutnaGodinaStudija.ToLower() == "prva" || trenutnaGodinaStudija == "1" || trenutnaGodinaStudija.ToLower() == "druga" || trenutnaGodinaStudija == "2" || trenutnaGodinaStudija.ToLower() == "treca" || trenutnaGodinaStudija == "3" || trenutnaGodinaStudija.ToLower() == "treća" || trenutnaGodinaStudija.ToLower() == "cetvrta" || trenutnaGodinaStudija == "4" || trenutnaGodinaStudija.ToLower() == "četvrta")
                 trenutnaGodinaStudija = trenutnaGodinaStudija;//STA OVDE DA RADIM
             else
                 return 5;
             Student student= new Student(prezime, ime, datum, adresa, kontaktTelefon,
                 emailAdresa, indeks, trenutnaGodinaStudija, status1);
             student.id = idSelected;
             ssluzba.UpdateStudent(student);
             SaveFile();
             return 0;
         }
         public int EditProfessor(string ime, string prezime, string datumRodjenja, string adresaStanovanja, string kontaktTelefon, string emailAdresa, string brojLicneKarte, string zvanje, string godineStaza, int idKatedre)
         {
             Datum datum = new Datum();
             if (datumRodjenja == null || !ssluzba.DatumRegex(datumRodjenja))
             {
                 return 1;
             }
             datum.setDatum(datumRodjenja);
             Adresa adresa = new Adresa();
             if (adresaStanovanja == null || !ssluzba.AdresaRegex(adresaStanovanja))
             {
                 return 2;
             }
             adresa.setAdresa(adresaStanovanja);
             if (ssluzba.GetAllKatedre().FindIndex(k => k.id == idKatedre) == -1)
             {
                 return 3;
             }
             Profesor profesor = new Profesor(ime, prezime, datum,
                    adresa, kontaktTelefon, emailAdresa, brojLicneKarte, zvanje, godineStaza, idKatedre);
             profesor.id = idSelected;
             ssluzba.UpdateProfessor(profesor);
             SaveFile();
             return 0;
         }
         public int EditPredmet(string naziv, int professorId, int espb, int sifra, int godinaStudija, string semestar)
         {
             if (ssluzba.GetAllProfessors().FindIndex(p => p.id == professorId) == -1)
             {
                 return 1;
             }
             if (godinaStudija > 4 || godinaStudija < 1)
                 return 2;
             Semestar semestar1;
             if (semestar.ToLower() == "letnji")
                 semestar1 = Semestar.Letnji;
             else if (semestar.ToLower() == "zimski")
                 semestar1 = Semestar.Zimski;
             else return 3;
             Predmet predmet = new Predmet(sifra, naziv, semestar1, godinaStudija, professorId, espb);
             predmet.id = idSelected;
             ssluzba.UpdatePredmet(predmet);
             SaveFile();
             return 0;
         }
        */
        //-------------------------------------------------------------------------------
        //-----------------------SEARCH-------------------------------------------------
        private void SearchEntity()//TESTIRATI DA LI RADIIIII
        {
            switch (currentTabIndex)
            {
                case 0:

                    if (string.IsNullOrWhiteSpace(search))
                    {
                        stranica = 0;
                        Update();

                    }
                    else

                    {
                        IEnumerable<Student> rezultati = null;
                        string[] searchniz = search.Split(' ');
                        if (searchniz.Length == 1)
                            rezultati = (ssluzba._studenti).Where(student => student.prezime.Contains(searchniz[0]));
                        else if (searchniz.Length == 2)
                        {
                            rezultati = (ssluzba._studenti)
                             .Where(student =>
                                        student.ime.Contains(searchniz[1]) &&
                                         student.prezime.Contains(searchniz[0])).ToList();
                        }
                        else if (searchniz.Length == 4)
                        {
                            rezultati = (ssluzba._studenti)
                               .Where(student =>
                               student.ime.Contains(searchniz[1]) &&
                               student.prezime.Contains(searchniz[0]) &&
                               student.brojIndeksa.ToString().Contains(searchniz[2] + " " + searchniz[3])).ToList();
                        }
                        if (rezultati.Any())
                        {
                            stranica = 0;
                            foundStudents = rezultati.ToList();
                            ShowSearchStudents(foundStudents);
                        }
                        else
                            MessageBox.Show("Ne postoji student sa kriterijumom pretrazivanja");
                    }
                    break;
                case 1:
                    if (string.IsNullOrWhiteSpace(search))
                    {
                        stranica = 0;
                        Update();

                    }
                    else
                    {
                        IEnumerable<Profesor> rezultati1 = null;
                        string[] searchniz = search.Split(' ');
                        if (searchniz.Length == 1)
                            rezultati1 = (ssluzba._profesori).Where(profesor => profesor.prezime.Contains(searchniz[0]));
                        else if (searchniz.Length == 2)
                        {

                            rezultati1 = (ssluzba._profesori)
                                         .Where(profesor =>
                                         profesor.ime.Contains(searchniz[1]) &&
                                         profesor.prezime.Contains(searchniz[0])).ToList();
                        }
                        if (rezultati1.Any())
                        {
                            stranica = 0;
                            foundProfessors = rezultati1.ToList();
                            ShowSearchProfessors(foundProfessors);     
                        }
                        else
                            MessageBox.Show("Ne postoji profesor sa kriterijumom pretrazivanja");
                    }
                    break;
                case 2:
                    if (string.IsNullOrWhiteSpace(search))
                    {
                        stranica = 0;
                        Update();

                    }
                    else
                    {
                        IEnumerable<Predmet> rezultati2 = null;
                        string[] searchniz = search.Split(' ');
                        if (searchniz.Length == 1)
                            rezultati2 = (ssluzba._predmeti).Where(predmet => predmet.naziv.Contains(searchniz[0]));
                        else if (searchniz.Length == 2)
                        {
                            rezultati2 = (ssluzba._predmeti)
                                          .Where(predmet =>
                                                predmet.sifra.ToString().Contains(searchniz[1]) &&
                                                predmet.naziv.Contains(searchniz[0])).ToList();
                        }
                        if (rezultati2.Any())
                        {
                            foundCourses = rezultati2.ToList();
                            stranica = 0;
                            ShowSearchCourses(foundCourses);

                        }
                        else
                            MessageBox.Show("Ne postoji predmet sa kriterijumom pretrazivanja");
                    }
                    break;
                case 3://ZA KATEDRU SEARCH
                    if (string.IsNullOrWhiteSpace(search))
                    {
                        stranica = 0;
                        Update();

                    }
                     else
                     {
                         IEnumerable<Katedra> rezultati3 = null;
                         string[] searchniz = search.Split(' ');
                         if (searchniz.Length == 1)
                             rezultati3 = (ssluzba._katedre).Where(predmet => predmet.naziv.Contains(searchniz[0]));
                         else if (searchniz.Length == 2)
                         {
                             rezultati3 = (ssluzba._katedre)
                                           .Where(predmet =>
                                                 predmet.sifra.ToString().Contains(searchniz[1]) &&
                                                 predmet.naziv.Contains(searchniz[0])).ToList();
                         }
                         if (rezultati3.Any())
                         {
                             foundDepartments = rezultati3.ToList();
                            stranica = 0;
                            ShowSearchDepartments(foundDepartments);

                        }
                         else
                             MessageBox.Show("Ne postoji Katedra sa kriterijumom pretrazivanja");
                     }
                    break;
                default:
                    break;
            }
        }
        void ShowSearchProfessors(List<Profesor>pronadjeni)
        {
            Profesori.Clear();
            int iteracija = 0;
            foreach (Profesor profesor in pronadjeni)
            {
                if (elementsPerPage * stranica <= iteracija)
                    Profesori.Add(new ProfesorDTO(profesor));
                Debug.WriteLine(profesor.ToString());
                if (Profesori.Count == elementsPerPage)
                    break;
                iteracija++;
            }
        }
        void ShowSearchCourses(List<Predmet>pronadjeni) {
            Predmeti.Clear();
            int iteracija = 0;
            foreach (Predmet predmet in pronadjeni)
            {
                if (elementsPerPage * stranica <= iteracija)
                    Predmeti.Add(new PredmetDTO(predmet));
                if (Predmeti.Count == elementsPerPage)
                    break;
                iteracija++;
                Debug.WriteLine(predmet.ToString());//OVDE PREKIDA AKO NE POSTOJI PROFESOR ODNOSNO SAMO TOSTRING NE RADI
            }
        }
        void ShowSearchStudents(List<Student>pronadjeni) {
            Studenti.Clear();
            int iteracija = 0;
            foreach (Student student in pronadjeni)
            {
                if (elementsPerPage * stranica <= iteracija)
                    Studenti.Add(new StudentDTO(student));
                if (Studenti.Count == elementsPerPage)
                    break;
                Debug.WriteLine(student.ToString());
                iteracija++;
            }
        }
        void ShowSearchDepartments(List<Katedra> pronadjeni)
        {
            Katedre.Clear();
            int iteracija = 0;
            foreach (Katedra katedra in pronadjeni)
            {
                if (elementsPerPage * stranica <= iteracija)
                Katedre.Add(new KatedraDTO(katedra));
                if (Katedre.Count == elementsPerPage)
                    break;
                iteracija++;
                Debug.WriteLine(katedra.ToString());
            }
        }
        //IZGLEDA DA OVE DOLE FUNKCIJE RADE ALI NA MUDA jer je kao brza ili sporija(valjda brza)mozda mogu i da prebacim gore sada kada sam sredio
        private void TabItem_GotFocus(object sender, RoutedEventArgs e)//NE ZNAM DA LI OVE FJE RADEEE mozda ako je brza od selecteditem
        {
            //Update();//RESETUJEM ZBOG SEARCHA
            //idSelected = 0;
        }

        private void TabItem_GotFocus_1(object sender, RoutedEventArgs e)
        {
            //Update();//RESETUJEM ZBOG SEARCHA
            //idSelected = 0;
        }

        private void TabItem_GotFocus_2(object sender, RoutedEventArgs e)
        {//GREEESKAA
           // Update();//RESETUJEM ZBOG SEARCHA
            // idSelected = 0;


        }
        private void Button_Click(object sender, RoutedEventArgs e)//ZA DESNO
        {
            switch (currentTabIndex)
            {
                case 0:
                    stranica++;
                    if (ssluzba._studenti.Count > elementsPerPage * stranica && search=="")
                    {
                            Update();
                    }
                    else if (foundStudents.Count > elementsPerPage * stranica && search != "")
                            ShowSearchStudents(foundStudents);
                    else
                        stranica--;
                    break;
                    case 1:
                    stranica++;
                    if (ssluzba._profesori.Count > elementsPerPage * stranica && search=="")
                    {
                            Update();
                    }
                    else if (foundProfessors.Count > elementsPerPage * stranica && search != "")
                            ShowSearchProfessors(foundProfessors);
                    else
                        stranica--;
                    break;
                case 2:
                    stranica++;
                    if (ssluzba._predmeti.Count > elementsPerPage * stranica && search=="")
                    {
                            Update();
                    }
                    else if(foundCourses.Count > elementsPerPage * stranica && search != "")
                             ShowSearchCourses(foundCourses);
                    else
                        stranica--;
                    break;
                    case 3:
                    stranica++;
                    if (ssluzba._katedre.Count > elementsPerPage * stranica && search=="")
                    {
                            Update();
                       
                    }
                    else if(foundDepartments.Count > elementsPerPage * stranica && search != "") 
                            ShowSearchDepartments(foundDepartments);
                    else
                        stranica--;
                    break;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//ZA LEVO
        {
            switch (currentTabIndex)
            {
                case 0:
                    if (stranica>0)
                    {
                        stranica--;
                        if (search == "")
                            Update();
                        else
                            ShowSearchStudents(foundStudents);
                    }
                    break;
                case 1:
                    if (stranica > 0)
                    {
                        stranica--;
                        if(search=="")
                        Update();
                        else
                        ShowSearchProfessors(foundProfessors);
                    }
                    break;
                case 2:
                    if (stranica > 0)
                    {
                        stranica--;
                        if (search == "")
                            Update();
                        else
                            ShowSearchCourses(foundCourses);
                    }
                    break;
                case 3:
                    if (stranica > 0)
                    {
                        stranica--;
                        if (search == "")
                            Update();
                        else
                            ShowSearchDepartments(foundDepartments);
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------
    }
}
