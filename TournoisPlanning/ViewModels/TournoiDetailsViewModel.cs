using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using TournoisPlanning;
using TournoisPlanning.Views;

namespace TournoisPlanning.ViewModels
{
    public class TournoiDetailsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Match> RootMatches { get; set; }

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

        public TournoiDetailsViewModel(Tournoi tournoi)
        {
            if (tournoi.Statut == "Terminé")
            {
                MatchView = new VainqueurView(tournoi);
                //SectionView = new StatistiquesTournoiView(tournoi);
            }
            else
            {
                var matchActuel = tournoi.GetMatchEnCours();
                MatchView = matchActuel != null ? new MatchEnCoursView(matchActuel) : new ProchainMatchView(tournoi.GetProchainMatch());
                SectionView = new ArbreTournoiView(tournoi);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
