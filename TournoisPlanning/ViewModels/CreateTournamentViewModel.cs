using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TournoisPlanning.Models;
using TournoisPlanning.Services;

namespace TournoisPlanning.ViewModels
{
    public class CreateTournamentViewModel : INotifyPropertyChanged
    {
        private readonly ITournoiService _tournoiService;
        private readonly INavigationService _navigationService;

        // Propriétés de base du tournoi
        private string _nom = string.Empty;
        private DateTime _dateDebut = DateTime.Now;
        private int _nombreEquipes;
        private int _dureeMatch = 90; // Valeur par défaut
        private string _lieu = string.Empty;
        private string _description = string.Empty;
        private string _prix = string.Empty;
        private string _saison = string.Empty; // Saison du tournoi

        // Type de tournoi
        private bool _isKnockoutType = true; // Par défaut
        private bool _isPouleType;
        private bool _isMixedType;

        // Fréquence des matchs
        private bool _isDailyFrequency;
        private bool _isWeeklyFrequency = true; // Par défaut
        private bool _isCustomFrequency;

        // Mode de planification
        private bool _isManualScheduling;
        private bool _isAutomaticScheduling = true; // Par défaut

        public CreateTournamentViewModel()
        {
            // Get registered services
            _tournoiService = ServiceLocator.GetService<ITournoiService>();
            _navigationService = ServiceLocator.GetService<INavigationService>();

            // Commandes
            CreerTournoiCommand = new RelayCommand(ExecuteCreerTournoi, CanExecuteCreerTournoi);
            AnnulerCommand = new RelayCommand(ExecuteAnnuler);
        }

        // Propriétés exposées au XAML
        public string Nom
        {
            get => _nom;
            set
            {
                if (_nom != value)
                {
                    _nom = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested(); // Refresh des commandes
                }
            }
        }
        public string Saison
        {
            get => _saison;
            set
            {
                if (_saison != value)
                {
                    _saison = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested(); // Refresh des commandes
                }
            }
        }

        public DateTime DateDebut
        {
            get => _dateDebut;
            set
            {
                if (_dateDebut != value)
                {
                    _dateDebut = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NombreEquipes
        {
            get => _nombreEquipes.ToString();
            set
            {
                if (int.TryParse(value, out int result) && _nombreEquipes != result)
                {
                    _nombreEquipes = result;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string DureeMatch
        {
            get => _dureeMatch.ToString();
            set
            {
                if (int.TryParse(value, out int result) && _dureeMatch != result)
                {
                    _dureeMatch = result;
                    OnPropertyChanged();
                }
            }
        }

        public string Lieu
        {
            get => _lieu;
            set
            {
                if (_lieu != value)
                {
                    _lieu = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Prix
        {
            get => _prix;
            set
            {
                if (_prix != value)
                {
                    _prix = value;
                    OnPropertyChanged();
                }
            }
        }

        // Type de tournoi
        public bool IsKnockoutType
        {
            get => _isKnockoutType;
            set
            {
                if (_isKnockoutType != value)
                {
                    _isKnockoutType = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsPouleType
        {
            get => _isPouleType;
            set
            {
                if (_isPouleType != value)
                {
                    _isPouleType = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsMixedType
        {
            get => _isMixedType;
            set
            {
                if (_isMixedType != value)
                {
                    _isMixedType = value;
                    OnPropertyChanged();
                }
            }
        }

        // Fréquence des matchs
        public bool IsDailyFrequency
        {
            get => _isDailyFrequency;
            set
            {
                if (_isDailyFrequency != value)
                {
                    _isDailyFrequency = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsWeeklyFrequency
        {
            get => _isWeeklyFrequency;
            set
            {
                if (_isWeeklyFrequency != value)
                {
                    _isWeeklyFrequency = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsCustomFrequency
        {
            get => _isCustomFrequency;
            set
            {
                if (_isCustomFrequency != value)
                {
                    _isCustomFrequency = value;
                    OnPropertyChanged();
                }
            }
        }

        // Mode de planification
        public bool IsManualScheduling
        {
            get => _isManualScheduling;
            set
            {
                if (_isManualScheduling != value)
                {
                    _isManualScheduling = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsAutomaticScheduling
        {
            get => _isAutomaticScheduling;
            set
            {
                if (_isAutomaticScheduling != value)
                {
                    _isAutomaticScheduling = value;
                    OnPropertyChanged();
                }
            }
        }

        // Commandes
        public ICommand CreerTournoiCommand { get; }
        public ICommand AnnulerCommand { get; }

        // Méthodes d'exécution des commandes
        private void ExecuteCreerTournoi(object parameter)
        {
            // Déterminer le type de tournoi sélectionné
            TypeTournoi typeTournoi = TypeTournoi.Knockout; // Valeur par défaut
            if (IsPouleType) typeTournoi = TypeTournoi.Poules;
            if (IsMixedType) typeTournoi = TypeTournoi.Mixed;

            // Déterminer la fréquence des matchs
            FrequenceMatch frequence = FrequenceMatch.Weekly; // Valeur par défaut
            if (IsDailyFrequency) frequence = FrequenceMatch.Daily;
            if (IsCustomFrequency) frequence = FrequenceMatch.Custom;

            // Créer l'objet tournoi
            var nouveauTournoi = new Tournoi
            {
                Nom = Nom,
                Type = typeTournoi,
                DateDebut = DateDebut,
                NombreEquipes = _nombreEquipes,
                FrequenceMatch = frequence,
                DureeMatch = _dureeMatch,
                Lieu = Lieu,
                Description = Description,
                Prix = Prix,
                PlanificationAutomatique = IsAutomaticScheduling,
                Saison = Saison,
            };

            try
            {
                // Enregistrer le tournoi
                _tournoiService.CreerTournoi(nouveauTournoi);
                
                // Si planification automatique, générer le planning des matchs
                if (IsAutomaticScheduling)
                {
                    _tournoiService.GenererPlanningAutomatique(nouveauTournoi.Id);
                }
                // Add this line to refresh the tournaments collection
                _tournoiService.RefreshTournaments();
                // Message de succès
                MessageBox.Show("Tournament created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Navigation vers la liste des tournois ou les détails du tournoi créé
                _navigationService.NavigateToTournamentDetails(nouveauTournoi.Id);
            }
            catch (Exception ex)
            {
                // Gestion des erreurs
                MessageBox.Show($"Error creating tournament: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecuteCreerTournoi(object parameter)
        {
            // Validation basique : vérifier que les champs obligatoires sont remplis
            return !string.IsNullOrWhiteSpace(Nom) && 
                   _nombreEquipes > 1 && 
                   _dureeMatch > 0 &&
                   !string.IsNullOrWhiteSpace(Lieu);
        }

        private void ExecuteAnnuler(object parameter)
        {
            // Retour à la page précédente
            _navigationService.GoBack();
        }

        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Classe RelayCommand pour gérer les commandes
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object>? _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter!);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter!);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}