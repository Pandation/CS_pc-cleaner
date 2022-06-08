using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LogicielNettoyagePC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DirectoryInfo winTemp;
        public DirectoryInfo appTemp;
        public MainWindow()
        {
            InitializeComponent();
            winTemp = new DirectoryInfo(@"C:\Windows\Temp");
            appTemp = new DirectoryInfo(System.IO.Path.GetTempPath());
        }

        //calcul taille dossier
        public long DirSize(DirectoryInfo dir)
        {
            return dir.GetFiles().Sum(f => f.Length) + dir.GetDirectories().Sum(di => DirSize(di));
        }

        //vider un dossier
        public void ClearTempData(DirectoryInfo dir)
        {
            foreach (FileInfo file in dir.GetFiles())
            {
                try
                {
                    file.Delete();
                    Console.WriteLine(file.FullName);
                }
                catch (Exception e)
                {
                    continue;
                }
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                try
                {

                    dir.Delete();
                    Console.WriteLine(dir.FullName);
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }

        public void AnalyseFolders()
        {
            Console.WriteLine("Début de l'analyse...");
            long totalSize = 0;

            totalSize += DirSize(winTemp) / 1000000;
            totalSize += DirSize(appTemp) / 1000000;

            espace.Content = totalSize + " Mb";
            titre.Content = "Analyse effectuée!";
            date.Content = DateTime.Today;
        }
        private void Button_MAJ_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logiciel à jour!", "Mise à jour", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Histo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("TODO: page Historique", "Historique", MessageBoxButton.OK, MessageBoxImage.Information);
            ChangeVisibilityIfActus();
        }

        private void Button_Website_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://florianbaumes.fr") { UseShellExecute = true });
        }

        private void Button_Clean_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Nettoyage en cours...");
            btnClean.Content = "Nettoyage en cours";

            Clipboard.Clear();

            try
            {
                ClearTempData(winTemp);
            }
            catch (Exception excep)
            {
                MessageBox.Show("Erreur : " + excep.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                ClearTempData(appTemp);
            }
            catch (Exception excep)
            {
                MessageBox.Show("Erreur : " + excep.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            btnClean.Content = "Nettoyage terminé".ToUpper();
            espace.Content = "0 Mb";
        }

        private void Button_Analyser_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("début de l'analyse");
            try
            {

                AnalyseFolders();
            }
            catch (Exception excep)
            {
                MessageBox.Show(excep.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeVisibilityIfActus()
        {
            actuTxt.Visibility = Visibility.Visible;
            bandeau.Visibility = Visibility.Visible;
        }
    }
}
