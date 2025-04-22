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
                var dashboardView = new DashboardView();
                dashboardView.TournoiSelected += AfficherDetailsTournoi;
                MainContentArea.Content = dashboardView;
            }

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
                //tournoisListView.TournoiSelected += AfficherDetailsTournoi;
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
