using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TournoisPlanning.Models;
using TournoisPlanning.Services;

namespace TournoisPlanning.ViewModels
{
    public class EquipeViewModel : INotifyPropertyChanged
    {
        private readonly IEquipeService _equipeService;
        private readonly int _tournoiId;
        private Equipes _nouvelleEquipe;
        private ObservableCollection<Equipes> _equipes;

        public EquipeViewModel(int tournoiId)
        {
            _tournoiId = tournoiId;
            _equipeService = ServiceLocator.GetService<IEquipeService>();

            // Initialize the new team with the tournament ID
            _nouvelleEquipe = new Equipes
            {
                TournoiId = tournoiId
            };

            // Load existing teams for this tournament
            ChargerEquipes();

            // Initialize commands
            AjouterEquipeCommand = new RelayCommand(param => ExecuteAjouterEquipe());
            SupprimerEquipeCommand = new RelayCommand(param => ExecuteSupprimerEquipe((int)param));
            FermerDialogueCommand = new RelayCommand(param => ExecuteFermerDialogue(param as Window));
        }

        private void ChargerEquipes()
        {
            var listeEquipes = _equipeService.ObtenirEquipesParTournoi(_tournoiId);
            Equipes = new ObservableCollection<Equipes>(listeEquipes);
        }

        private void ExecuteAjouterEquipe()
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(NouvelleEquipe.Nom))
                {
                    MessageBox.Show("Le nom de l'équipe est obligatoire.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Create the team
                _equipeService.CreerEquipe(NouvelleEquipe);

                // Refresh the list
                ChargerEquipes();

                // Reset the form with a new team object
                NouvelleEquipe = new Equipes
                {
                    TournoiId = _tournoiId
                };

                // Success message
                MessageBox.Show("Équipe ajoutée avec succès!", "Succès",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création de l'équipe: {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteSupprimerEquipe(int equipeId)
        {
            try
            {
                // Confirmation dialog
                var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette équipe?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Delete the team
                    _equipeService.SupprimerEquipe(equipeId);

                    // Refresh the list
                    ChargerEquipes();

                    MessageBox.Show("Équipe supprimée avec succès!", "Succès",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la suppression de l'équipe: {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteFermerDialogue(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        public Equipes NouvelleEquipe
        {
            get => _nouvelleEquipe;
            set
            {
                if (_nouvelleEquipe != value)
                {
                    _nouvelleEquipe = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Equipes> Equipes
        {
            get => _equipes;
            set
            {
                if (_equipes != value)
                {
                    _equipes = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AjouterEquipeCommand { get; }
        public ICommand SupprimerEquipeCommand { get; }
        public ICommand FermerDialogueCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}