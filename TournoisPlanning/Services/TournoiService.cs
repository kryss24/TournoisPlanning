using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using TournoisPlanning.Models;

namespace TournoisPlanning.Services
{
    // Implémentation basique du service de tournois
    // Implémentation du service de tournois utilisant des fichiers
    public class TournoiService : ITournoiService
    {
        private readonly string _filePath;
        private ObservableCollection<Tournoi> _tournois;
        private int _nextId = 1;

        public TournoiService()
        {
            // Définir le chemin du fichier
            string appDataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TournoisPlanning");

            // Créer le dossier s'il n'existe pas
            if (!Directory.Exists(appDataFolder))
            {
                Directory.CreateDirectory(appDataFolder);
            }

            _filePath = Path.Combine(appDataFolder, "tournois.json");

            // Charger les tournois existants
            LoadTournaments();
        }

        private void LoadTournaments()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    _tournois = JsonSerializer.Deserialize<ObservableCollection<Tournoi>>(json) ?? new ObservableCollection<Tournoi>();

                    // Déterminer le prochain ID
                    if (_tournois.Any())
                    {
                        _nextId = _tournois.Max(t => t.Id) + 1;
                    }
                }
                else
                {
                    _tournois = new ObservableCollection<Tournoi>();
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, commencer avec une liste vide
                _tournois = new ObservableCollection<Tournoi>();
                Console.WriteLine($"Error loading tournaments: {ex.Message}");
            }
        }

        private void SaveTournaments()
        {
            try
            {
                string json = JsonSerializer.Serialize(_tournois, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save tournaments: {ex.Message}", ex);
            }
        }

        public int CreerTournoi(Tournoi tournoi)
        {
            tournoi.Id = _nextId++;
            _tournois.Add(tournoi);
            SaveTournaments();
            return tournoi.Id;
        }

        public void GenererPlanningAutomatique(int tournoiId)
        {
            var tournoi = ObtenirTournoi(tournoiId);
            if (tournoi == null)
                throw new ArgumentException($"Tournoi avec ID {tournoiId} non trouvé");

            // Logique de génération du planning automatique
            // Dans une implémentation complète, vous créeriez des matchs et les ajouteriez au tournoi
            Console.WriteLine($"Planning généré pour le tournoi {tournoi.Nom}");
        }

        public Tournoi ObtenirTournoi(int id)
        {
            return _tournois.FirstOrDefault(t => t.Id == id);
        }

        public List<Tournoi> ObtenirTousTournois()
        {
            return new List<Tournoi>(_tournois);
        }

        public void ModifierTournoi(Tournoi tournoi)
        {
            // Remplacement de FindIndex par une recherche manuelle
            var existingTournoi = _tournois.FirstOrDefault(t => t.Id == tournoi.Id);
            if (existingTournoi == null)
                throw new ArgumentException($"Tournoi avec ID {tournoi.Id} non trouvé");

            // Mise à jour des propriétés du tournoi existant
            int index = _tournois.IndexOf(existingTournoi);
            _tournois[index] = tournoi;
            SaveTournaments();
        }

        public void SupprimerTournoi(int id)
        {
            var tournoi = ObtenirTournoi(id);
            if (tournoi != null)
            {
                _tournois.Remove(tournoi);
                SaveTournaments();
            }
        }
        public void RefreshTournaments()
        {
            DbTournoiService dbTournoiService = new DbTournoiService(new Config.DBConn("127.0.0.1", "adminTM", "firstproject", "tournoisManagement"));

            // Load tournaments from database
            var tournaments = dbTournoiService.ObtenirTousTournois(); // Your database access method

            // Clear and reload the collection
            _tournois.Clear();
            foreach (var tournament in tournaments)
            {
                _tournois.Add(tournament);
            }

            // If you're using an event mechanism
            OnTournamentsChanged();
        }
        public event EventHandler TournamentsChanged;

        protected virtual void OnTournamentsChanged()
        {
            TournamentsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    // Implémentation basique du service de navigation
    public class NavigationService : INavigationService
    {
        public void GoBack()
        {
            // Essayez d'obtenir la fenêtre active
            if (Application.Current.MainWindow is Window mainWindow)
            {
                // Si la fenêtre est une fenêtre de détails ou un formulaire, fermez-la
                mainWindow.Close();
            }
        }

        public void NavigateToTournamentDetails(int tournoiId)
        {
            // Implémentation simplifiée: afficher un message avec l'ID du tournoi
            MessageBox.Show($"Navigation vers les détails du tournoi ID: {tournoiId}",
                "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);

            // Dans une vraie implémentation, vous créeriez et afficheriez une nouvelle fenêtre
            // var detailsWindow = new TournamentDetailsWindow(tournoiId);
            // detailsWindow.Show();
        }

        public void NavigateToTournamentsList()
        {
            // Implémentation simplifiée: afficher un message
            MessageBox.Show("Navigation vers la liste des tournois",
                "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);

            // Dans une vraie implémentation, vous créeriez et afficheriez une nouvelle fenêtre
            // var listWindow = new TournamentListWindow();
            // listWindow.Show();
        }
    }
}