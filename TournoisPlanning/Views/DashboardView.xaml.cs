using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using OxyPlot.Series;
using OxyPlot;
using TournoisPlanning.Views;
using TournoisPlanning.Models;

namespace TournoisPlanning.Views
{
    /// <summary>
    /// Logique d'interaction pour DashboardView.xaml
    /// </summary>
    /// 
    public partial class DashboardView : UserControl
    {
        public event Action<Tournoi>? TournoiSelected;
        public ObservableCollection<Tournoi> Tournois { get; set; }
        public DashboardView()
        {
            InitializeComponent();
            Tournois = new ObservableCollection<Tournoi>
                {
                    new Tournoi { Nom = "Coupe ENSPD", Type = "Élimination directe", NombreEquipes = 16, Statut = "En cours", Matches = new List<Match>(){
                            new Match { Equipe1 = "GIT", Equipe2 = "GRT", Statut = "En cours", ScoreEquipe1 = 0, ScoreEquipe2 = 0 },
                            new Match { Equipe1 = "GESI", Equipe2 = "GCI", Statut = "En cours", ScoreEquipe1 = 0, ScoreEquipe2 = 0 },
                            new Match { Equipe1 = "Medecine", Equipe2 = "Petrochimie", Statut = "En cours", ScoreEquipe1 = 0, ScoreEquipe2 = 0 },
                            new Match { Equipe1 = "Alternance", Equipe2 = "Prof", Statut = "En cours", ScoreEquipe1 = 0, ScoreEquipe2 = 0 },
                        }
                    },
                    new Tournoi { Nom = "Championnat Réseau", Type = "Poules", NombreEquipes = 8, Statut = "En attente", Matches = new List<Match>(){
                            new Match { Equipe1 = "GRT3", Equipe2 = "GRT4", Statut = "En cours", ScoreEquipe1 = 0, ScoreEquipe2 = 0 },
                        }
                    },
                    new Tournoi { Nom = "Tournoi Dev", Type = "Élimination directe", NombreEquipes = 12, Statut = "Terminé", Matches = new List<Match>(){
                            new Match {Equipe1 = "GLO3", Equipe2 = "GLO4", Statut = "En cours", ScoreEquipe1 = 0, ScoreEquipe2 = 0},
                        } }
                };
            DataContext = this;
            InitializeGraphs();
        }
        private void InitializeGraphs()
        {
            try
            {
                // Graphique pour les Tournois en cours
                var tournoisModel = new PlotModel { Title = "Tournois en Cours" };
                var tournoisSeries = new BarSeries
                {
                    ItemsSource = new[] { new BarItem { Value = 3 }, new BarItem { Value = 7 }, new BarItem { Value = 5 } },
                    LabelPlacement = LabelPlacement.Outside,
                    LabelFormatString = "{0}"
                };
                tournoisModel.Series.Add(tournoisSeries);
                TournoisGraph.Model = tournoisModel;

                // Graphique pour les matchs joués
                var matchsModel = new PlotModel { Title = "Matchs Joués" };
                var matchsSeries = new LineSeries
                {
                    ItemsSource = new[] { new DataPoint(1, 10), new DataPoint(2, 20), new DataPoint(3, 30) },
                    LineStyle = LineStyle.Solid,
                    MarkerType = MarkerType.Circle
                };
                matchsModel.Series.Add(matchsSeries);
                MatchsGraph.Model = matchsModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'initialisation des graphiques: " + ex.Message);
            }
        }
        private void VoirDetails_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var tournoi = button?.DataContext as Tournoi;

            if (tournoi != null)
            {
                var detailsView = new TournoiDetailsView
                {
                    DataContext = tournoi
                };

                // Assuming MainContentArea is a ContentControl in your DashboardView
                TournoiSelected?.Invoke(tournoi);
                //MainContentArea.Content = detailsView;
            }
        }
    }
}
