using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using MySql.Data.MySqlClient;
using TournoisPlanning.Config;
using TournoisPlanning.Models;
using TournoisPlanning.Services;
using TournoisPlanning.Views;

namespace TournoisPlanning.ViewModels
{
    public class TournoiDetailsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TournoisPlanning.Models.Match> RootMatches { get; set; }
        public ObservableCollection<Equipes> Equipes { get; set; }
        public Tournoi tournoi1 { get; set; }
        private UserControl _matchView;
        private UserControl _sectionView;

        public UserControl MatchView
        {
            get => _matchView;
            set
            {
                _matchView = value;
                OnPropertyChanged();
            }
        }

        public UserControl SectionView
        {
            get => _sectionView;
            set
            {
                _sectionView = value;
                OnPropertyChanged();
            }
        }

        public event Action OpenSettingsRequested;
        public event Action GoBackRequested;

        public ICommand OpenSettingsCommand => new RelayCommand(_ => OpenSettingsRequested?.Invoke());
        public ICommand GoBackToViewCommand => new RelayCommand(_ => GoBackRequested?.Invoke());

        // Commande pour l'édition des équipes
        public ICommand EditTeamsCommand { get; private set; }
        public ICommand GenerateMatchesCommand { get; private set; }
        private readonly DBConn dBConn;

        public TournoiDetailsViewModel(Tournoi tournoi, DBConn dBCon)
        {
            RootMatches = new ObservableCollection<TournoisPlanning.Models.Match>();
            Equipes = new ObservableCollection<Equipes>();
            //MessageBox.Show(tournoi.Statut
            tournoi1 = tournoi;
            dBConn = dBCon;
            if (tournoi.Statut == "Terminé")
            {
                MatchView = new VainqueurView(tournoi);
                SectionView = new ArbreTournoiView(tournoi);
            }
            else
            {
                var matchActuel = tournoi.GetMatchEnCours();
                MatchView = matchActuel != null ? new MatchEnCoursView(matchActuel) : new ProchainMatchView(tournoi.GetProchainMatch());
                SectionView = new ArbreTournoiView(tournoi);
            }
            foreach (var match in tournoi.Matches)
            {
                RootMatches.Add(match);
            }
            foreach (var equipe in tournoi.Equipes)
            {
                Equipes.Add(equipe);
            }
            // Initialisez la commande
            // Update the RelayCommand initialization to match the expected Action<object> signature
            EditTeamsCommand = new RelayCommand(_ => OpenTeamsEditor());
            GenerateMatchesCommand = new RelayCommand(_ => GenerateMatches());
            this.dBConn = dBConn;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private void OpenTeamsEditor()
        {
            MessageBox.Show("Gestion des équipes");
            // Ouvrir la fenêtre de dialogue pour gérer les équipes
            //var gestionEquipesDialog = new GestionEquipesDialog(tournoi1.Id);
            //gestionEquipesDialog.Owner = Application.Current.MainWindow;

            // Si vous avez besoin de rafraîchir les équipes après la fermeture du dialogue
            //if (gestionEquipesDialog.ShowDialog() == true)
            //{
            //    // Rafraîchir les équipes si nécessaire
            //    LoadEquipes();
            //}
        }

        // Cette méthode génère les paires de matchs en fonction des équipes enregistrées
        private void GenerateMatches()
        {
            MessageBox.Show("Génération des matchs démarrée");
            try
            {
                // Vérification du nombre d'équipes
                if (Equipes == null || Equipes.Count < 2)
                {
                    MessageBox.Show("Au moins deux équipes sont nécessaires pour générer des matchs.",
                        "Impossible de générer", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Générer toutes les combinaisons possibles de matchs (chaque équipe joue contre chaque autre équipe)
                List<TournoisPlanning.Models.Match> nouveauxMatchs = new List<TournoisPlanning.Models.Match>();

                for (int i = 0; i < Equipes.Count; i++)
                {
                    for (int j = i + 1; j < Equipes.Count; j++)
                    {
                        // Créer un nouveau match
                        var match = new TournoisPlanning.Models.Match
                        {
                            Equipe1 = Equipes[i],
                            Equipe2 = Equipes[j],
                            ScoreEquipe1 = 0,
                            ScoreEquipe2 = 0,
                            //TournoiId = TournoiId, // Supposant que TournoiId existe dans le ViewModel
                            Lieu = "À déterminer", // Par défaut
                            EstTermine = false,
                            Statut = "À venir",
                            Date = DateTime.Now.AddDays(nouveauxMatchs.Count + 1), // Date temporaire
                            Round = 1 // Premier tour par défaut
                        };

                        nouveauxMatchs.Add(match);
                    }
                }

                // Enregistrer les matchs dans la base de données
                SaveMatchesToDatabase(nouveauxMatchs);
                RefreshTournaments();
                // Mettre à jour la collection observable
                foreach (var match in nouveauxMatchs)
                {
                    RootMatches.Add(match);
                }

                MessageBox.Show($"{nouveauxMatchs.Count} matchs ont été générés avec succès.",
                    "Génération réussie", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la génération des matchs: {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void RefreshTournaments()
        {
            MessageBox.Show("refresh");
            DBConn dBConn = new DBConn("127.0.0.1", "adminTM", "firstproject", "tournoisManagement");
            DbTournoiService dbTournoiService = new DbTournoiService(dBConn);
            // Récupérer tous les tournois depuis la base de données
            var tournois = dbTournoiService.ObtenirTousTournois();

            // Mettre à jour la collection observable dans le ViewModel
            Application.Current.Dispatcher.Invoke(() =>
            {
                var dashboardViewModel = ServiceLocator.GetService<Dashboard>();
                if (dashboardViewModel != null)
                {
                    dashboardViewModel.Tournois.Clear();
                    foreach (var tournoi in tournois)
                    {
                        dashboardViewModel.Tournois.Add(tournoi);
                        if(tournoi1.Id == tournoi.Id)
                        {
                            tournoi1 = tournoi;
                        }
                    }
                    foreach (var tournois in dashboardViewModel.Tournois)
                    {
                        tournois.InitialiserProchainMatch();
                    }
                }
                
            });
        }
        // Méthode pour sauvegarder les matchs dans la base de données
        private void SaveMatchesToDatabase(List<TournoisPlanning.Models.Match> matches)
        {
            try
            {
                // Pour chaque match, insérer dans la base de données
                foreach (var match in matches)
                {
                    string query = @"INSERT INTO matchs (Equipe1Id, Equipe2Id, ScoreEquipe1, ScoreEquipe2, Terrain, 
                           Statut, DateMatch, TournoiId) 
                           VALUES (@equipe1, @equipe2, @scoreEquipe1, @scoreEquipe2, @lieu, 
                           @statut, @date, @tournoiId)";

                    using (var connection = dBConn.GetConnection())
                    {
                        connection.Open();
                        using (var command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@equipe1", match.Equipe1.Id);
                            command.Parameters.AddWithValue("@equipe2", match.Equipe2.Id);
                            command.Parameters.AddWithValue("@scoreEquipe1", match.ScoreEquipe1);
                            command.Parameters.AddWithValue("@scoreEquipe2", match.ScoreEquipe2);
                            command.Parameters.AddWithValue("@lieu", match.Lieu);
                            command.Parameters.AddWithValue("@statut", match.Statut);
                            command.Parameters.AddWithValue("@date", match.Date);
                            command.Parameters.AddWithValue("@tournoiId", tournoi1.Id);

                            if (command.ExecuteNonQuery() == 0)
                            {
                                throw new KeyNotFoundException($"Tournoi avec ID {tournoi1.Id} non trouvé");
                            }
                        }
                    }
                }

                // Afficher un message de succès si tout se passe bien
                //MessageBox.Show($"{matches.Count} matchs ont été enregistrés avec succès dans la base de données.",
                //        "Sauvegarde réussie", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // En cas d'erreur, afficher un message d'erreur détaillé
                MessageBox.Show($"Erreur lors de la sauvegarde des matchs: {ex.Message}",
                        "Erreur de sauvegarde", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
