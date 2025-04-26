using System.Collections.ObjectModel;
using System.Data.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OxyPlot; // Ensure this namespace is included
using OxyPlot.Series; // Ensure this namespace is included
using TournoisPlanning.Config;
using TournoisPlanning.Models;
using TournoisPlanning.Services;
using TournoisPlanning.Views;

namespace TournoisPlanning
    {
        public partial class Dashboard : Window
        {
            public ObservableCollection<Tournoi> Tournois { get; set; }
            DBConn bConn;
            public Dashboard(DBConn db)
            {
                InitializeComponent();
                ServiceLocator.RegisterService<Dashboard>(this);
                // Create database connection
                bConn = db;
                //// Register services
                //ServiceLocator.RegisterService<ITournoiService>(new DbTournoiService(db));
                //ServiceLocator.RegisterService<INavigationService>(new TournoisPlanning.Services.NavigationService());
                var tournoiService = ServiceLocator.GetService<ITournoiService>();
                var tournoisDepuisBd = tournoiService.ObtenirTousTournois(); // récupère tous les tournois depuis la BD

                Tournois = new ObservableCollection<Tournoi>(tournoisDepuisBd);


                foreach (var tournois in Tournois)
                {
                    tournois.InitialiserProchainMatch();
                }
                DataContext = this;
                var dashboardView = new DashboardView();
                MainContentArea.Content = dashboardView;
            }

            private void Dashboard_Click(object sender, RoutedEventArgs e)
            {
                var dashboardView = new DashboardView();
                MainContentArea.Content = dashboardView;
            }

            private void Tournois_Click(object sender, RoutedEventArgs e)
            {
                //MainContentArea.Content = new ListeTournoisView();
                var tournoisListView = new ListeTournoisView(bConn);
                //tournoisListView.TournoiSelected += AfficherDetailsTournoi;
                MainContentArea.Content = tournoisListView;
            }

            private void Calandrier_Click(object sender, RoutedEventArgs e)
            {
                //MainContentArea.Content = new GestionEquipesView();
            }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }

    }
