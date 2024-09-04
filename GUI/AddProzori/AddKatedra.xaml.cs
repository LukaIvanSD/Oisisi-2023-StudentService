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
    /// Interaction logic for AddKatedra.xaml
    /// </summary>
    public partial class AddKatedra : Window
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private StudentskaSluzbaDAO ssluzba;
        public KatedraDTO Katedra { get; set; }
        public AddKatedra(StudentskaSluzbaDAO sluzba)
        {
            Katedra = new KatedraDTO();
            ssluzba = sluzba;
            InitializeComponent();
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
            if (Katedra.IsValid)
            {
                Katedra.SefId = -1;
                ssluzba.AddKatedra(Katedra.ToKatedra());
                Close();
            }
            else
            {
                MessageBox.Show("Department can not be created. Not all fields are valid.");
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
