using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using TournoisPlanning;
using TournoisPlanning.Config;

namespace TournoisPlanning
{
    public partial class MainWindow : Window
    {
        //private TournamentManager manager;
        DBConn db = new DBConn("127.0.0.1", "adminTM", "firstproject", "tournoisManagement");

        public MainWindow()
        {
            InitializeComponent();
            db.OpenConnection();
            //manager = new TournamentManager();
            //DrawTournament(manager.RootMatches);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            
            int count = (int)db.VerifiedUser(txtUser.Text, txtPass.Password);
            if (count > 0)
            {
                this.Hide();
                var dashboard = new Dashboard();
                dashboard.Show();
            } else
            {
                MessageBox.Show("Information Errone");
            }
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
