using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TournoisPlanning.Models;
using TournoisPlanning.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TournoisPlanning.Views
{
    public partial class calandrier : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<Tournoi> Tournois { get; set; } = new ObservableCollection<Tournoi>();
        private ITournoiService _tournoiService;

        public calandrier()
        {
            InitializeComponent();
            DataContext = this;
            _tournoiService = ServiceLocator.GetService<ITournoiService>();
            CurrentDate = DateTime.Now;
            InitializeData();
            UpdateCalendar();
        }

        #region Properties

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
                OnPropertyChanged(nameof(CurrentMonthYear));
                UpdateCalendar();
            }
        }

        public string CurrentMonthYear => CurrentDate.ToString("MMMM yyyy", new CultureInfo("fr-FR"));

        private ObservableCollection<CalendarDay> _calendarDays = new ObservableCollection<CalendarDay>();
        public ObservableCollection<CalendarDay> CalendarDays
        {
            get => _calendarDays;
            set
            {
                _calendarDays = value;
                OnPropertyChanged(nameof(CalendarDays));
            }
        }

        private ObservableCollection<TournamentType> _tournamentTypes = new ObservableCollection<TournamentType>();
        public ObservableCollection<TournamentType> TournamentTypes
        {
            get => _tournamentTypes;
            set
            {
                _tournamentTypes = value;
                OnPropertyChanged(nameof(TournamentTypes));
            }
        }

        #endregion

        #region Data Models

        public class CalendarDay : INotifyPropertyChanged
        {
            public int Day { get; set; }
            public DateTime Date { get; set; }
            public bool IsCurrentMonth { get; set; }
            public bool IsToday { get; set; }
            public ObservableCollection<Match> Matches { get; set; } = new ObservableCollection<Match>();

            public bool HasMatches => Matches.Count > 0;
            public int MatchCount => Matches.Count;

            public Color BackgroundColor => IsCurrentMonth ? Colors.White : Color.FromRgb(249, 250, 251);
            public Color DateBackgroundColor => IsToday ? Color.FromRgb(139, 92, 246) : Colors.Transparent;
            public Brush DateForeground => IsToday ? Brushes.White : (IsCurrentMonth ? Brushes.Black : Brushes.Gray);

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class TournamentType
        {
            public Tournoi tournoi { get; set; }
            public Brush TextColor { get; set; }
            public Brush Color { get; set; }
        }

        #endregion

        #region Event Handlers

        private void PrevMonthButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentDate = CurrentDate.AddMonths(-1);
        }

        private void NextMonthButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentDate = CurrentDate.AddMonths(1);
        }

        private void CreateMatchButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Fonctionnalité 'Nouveau Match' à implémenter", "Information");
        }

        #endregion

        #region Logic

        private void InitializeData()
        {
            var tournoisDepuisBd = _tournoiService.ObtenirTousTournois();
            Tournois = new ObservableCollection<Tournoi>(tournoisDepuisBd);

            TournamentTypes.Clear();
            foreach (var t in Tournois)
            {
                TournamentTypes.Add(new TournamentType
                {
                    tournoi = t,
                    TextColor = new SolidColorBrush(Color.FromRgb(107, 33, 168)),
                    Color = new SolidColorBrush(Color.FromRgb(100, 44, 25)),
                });
            }
        }

        private void UpdateCalendar()
        {
            CalendarDays.Clear();

            var firstDayOfMonth = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
            var startDate = firstDayOfMonth.AddDays(-(int)firstDayOfMonth.DayOfWeek);
            var today = DateTime.Today;

            // Précharger tous les matchs à l’avance
            var allMatches = new List<Match>();
            foreach (var tournoi in Tournois)
            {
                var matchs = _tournoiService.ObtenirMatchsPourTournoi(tournoi.Id);
                allMatches.AddRange(matchs);
            }

            for (int i = 0; i < 42; i++) // 6 semaines x 7 jours
            {
                var date = startDate.AddDays(i);
                var day = new CalendarDay
                {
                    Day = date.Day,
                    Date = date,
                    IsCurrentMonth = date.Month == CurrentDate.Month,
                    IsToday = date.Date == today
                };

                var matchesForDay = allMatches.Where(m => DateOnly.FromDateTime(m.Date) == DateOnly.FromDateTime(date));

                foreach (var match in matchesForDay)
                {
                    day.Matches.Add(match);
                }

                CalendarDays.Add(day);
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
