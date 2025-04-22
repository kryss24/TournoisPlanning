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

namespace TournoisPlanning.Views
{
    /// <summary>
    /// Logique d'interaction pour ListeTournoisView.xaml
    /// </summary>
    public partial class ListeTournoisView : UserControl
    {
        public event Action<Tournoi>? TournoiSelected;
        public ListeTournoisView()
        {
            InitializeComponent();
        }
        private void VoirTournoi_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is Tournoi tournoi)
            {
                // L'événement est toujours déclenché pour d'autres logiques, mais dans ce cas nous affichons le détail
                TournoiSelected?.Invoke(tournoi);

                // Obtenez une instance du TournoiDetailsPanel
                var tournoiDetailsPanel = new TournoiDetailsPanel();

                // Affectez le DataContext pour lier les données du tournoi au contrôle
                //tournoiDetailsPanel.DataContext = tournoi;
                tournoiDetailsPanel.SetTournoi(tournoi);

                // Supposons que vous avez un ContentControl pour afficher le détail dans votre fenêtre parent
                // Exemple : "ContentControlArea" dans le XAML parent
                var parentWindow = Window.GetWindow(this);
                if (parentWindow is Dashboard dashboard)
                {
                    //tournoiDetailsPanel.SetTournoi(tournoi);    
                    dashboard.MainContentArea.Content = tournoiDetailsPanel;
                }
            }
        }

    }
}
