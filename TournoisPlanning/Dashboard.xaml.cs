using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using OxyPlot; // Ensure this namespace is included
using OxyPlot.Series; // Ensure this namespace is included
using TournoisPlanning;
using TournoisPlanning.Views;

namespace TournoisPlanning
{
    public partial class Dashboard : Window
    {
        public ObservableCollection<Tournoi> Tournois { get; set; }

        public Dashboard()
        {
            InitializeComponent();
            Tournois = new ObservableCollection<Tournoi>
        {
            new Tournoi { Nom = "Coupe ENSPD", Type = "Élimination directe", NombreEquipes = 16, Statut = "En cours" },
            new Tournoi { Nom = "Championnat Réseau", Type = "Poules", NombreEquipes = 8, Statut = "En attente" },
            new Tournoi { Nom = "Tournoi Dev", Type = "Élimination directe", NombreEquipes = 12, Statut = "Terminé" }
        };

            DataContext = this;
            var dashboardView = new DashboardView();
            dashboardView.TournoiSelected += AfficherDetailsTournoi;
            MainContentArea.Content = dashboardView;
        }

        //private void InitializeGraphs()
        //{
        //    try
        //    {
        //        // Graphique pour les Tournois en cours
        //        var tournoisModel = new PlotModel { Title = "Tournois en Cours" };
        //        var tournoisSeries = new BarSeries
        //        {
        //            ItemsSource = new[] { new BarItem { Value = 3 }, new BarItem { Value = 7 }, new BarItem { Value = 5 } },
        //            LabelPlacement = LabelPlacement.Outside,
        //            LabelFormatString = "{0}"
        //        };
        //        tournoisModel.Series.Add(tournoisSeries);
        //        TournoisGraph.Model = tournoisModel;

        //        // Graphique pour les matchs joués
        //        var matchsModel = new PlotModel { Title = "Matchs Joués" };
        //        var matchsSeries = new LineSeries
        //        {
        //            ItemsSource = new[] { new DataPoint(1, 10), new DataPoint(2, 20), new DataPoint(3, 30) },
        //            LineStyle = LineStyle.Solid,
        //            MarkerType = MarkerType.Circle
        //        };
        //        matchsModel.Series.Add(matchsSeries);
        //        MatchsGraph.Model = matchsModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Erreur lors de l'initialisation des graphiques: " + ex.Message);
        //    }
        //}

        //private void VoirDetails_Click(object sender, RoutedEventArgs e)
        //{
        //    var button = sender as Button;
        //    var tournoi = button?.DataContext as Tournoi;

        //    if (tournoi != null)
        //    {
        //        var detailsView = new TournoiDetailsView
        //        {
        //            DataContext = tournoi
        //        };

        //        MainContentArea.Content = detailsView;
        //    }
        //}
        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            var dashboardView = new DashboardView();
            dashboardView.TournoiSelected += AfficherDetailsTournoi;
            MainContentArea.Content = dashboardView;
        }

        private void Tournois_Click(object sender, RoutedEventArgs e)
        {
            //MainContentArea.Content = new ListeTournoisView();
            var tournoisListView = new ListeTournoisView();
            tournoisListView.TournoiSelected += AfficherDetailsTournoi;
            MainContentArea.Content = tournoisListView;
        }
        private void AfficherDetailsTournoi(Tournoi tournoi)
        {
            var detailsView = new TournoiDetailsView
            {
                DataContext = tournoi
            };
            MainContentArea.Content = detailsView;
        }

        private void Calandrier_Click(object sender, RoutedEventArgs e)
        {
            //MainContentArea.Content = new GestionEquipesView();
        }
    }

}
