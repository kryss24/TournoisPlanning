using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TournoisPlanning.Views
{
    /// <summary>
    /// Logique d'interaction pour ListeTournoisView.xaml
    /// </summary>
    public partial class ListeTournoisView : UserControl
    {
        public event Action<Tournoi>? TournoiSelected;
        public ListeTournoisView()
        {
            InitializeComponent();
        }
        private void VoirTournoi_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is Tournoi tournoi)
            {
                TournoiSelected?.Invoke(tournoi);
            }
        }
    }
}
