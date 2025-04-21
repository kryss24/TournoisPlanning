using System.Windows;
using System.Windows.Controls;

namespace TournoisPlanning
{
    public partial class WinnerSelectionWindow : Window
    {
        public string GagnantChoisi { get; private set; }

        public WinnerSelectionWindow(string eq1, string eq2)
        {
            InitializeComponent();
            BtnEquipe1.Content = eq1;
            BtnEquipe2.Content = eq2;
        }

        private void BtnEquipe_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            GagnantChoisi = button.Content.ToString();
            DialogResult = true;
            Close();
        }
    }
}
