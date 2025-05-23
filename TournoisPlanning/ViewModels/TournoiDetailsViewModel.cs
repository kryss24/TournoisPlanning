﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using TournoisPlanning.Models;
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
            RootMatches = new ObservableCollection<Match>();
            //MessageBox.Show(tournoi.Statut);
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
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
