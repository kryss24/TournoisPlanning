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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TournoisPlanning.ViewModels;
using TournoisPlanning.Views;
using TournoisPlanning.Models;
using System.Windows.Media.Animation;
using TournoisPlanning.Config;

namespace TournoisPlanning.Views
{
    /// <summary>
    /// Logique d'interaction pour TournoiDetailsPanel.xaml
    /// </summary>
    public partial class TournoiDetailsPanel : UserControl
    {
        Tournoi tournoi1;
        private DBConn dBCon;

        public TournoiDetailsPanel(DBConn dBConn)
        {
            InitializeComponent();
            dBCon = dBConn;
        }
        public void SetTournoi(Tournoi tournoi)
        {
            // Définir le DataContext à une instance de TournoiDetailsViewModel
            DataContext = new TournoiDetailsViewModel(tournoi, dBCon);
            tournoi1 = tournoi;
            tournoi1.InitialiserProchainMatch();
            //DataContext = tournoi;
        }
        private void ShowSettings(object sender, RoutedEventArgs e)
        {
            settingsPan.Visibility = Visibility.Visible;
            viewPan.Visibility = Visibility.Collapsed;
        }
        private void ShowView(object sender, RoutedEventArgs e)
        {
            settingsPan.Visibility = Visibility.Collapsed;
            viewPan.Visibility = Visibility.Visible;
        }
        private void AddEquipe_Click(object sender, RoutedEventArgs e) {
            GestionEquipesDialog gestionEquipesDialog = new GestionEquipesDialog(tournoi1.Id);
            var parentWindow = Window.GetWindow(this);
            if (parentWindow is Dashboard dashboard)
            {
                //tournoiDetailsPanel.SetTournoi(tournoi);    
                dashboard.MainContentArea.Content = gestionEquipesDialog;
            }
        }
        private void Reload_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("Change");
        }
    }
}

