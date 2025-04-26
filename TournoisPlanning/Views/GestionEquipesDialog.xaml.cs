// TournoisPlanning.Views/GestionEquipesDialog.xaml.cs
using System.Windows.Controls;
using TournoisPlanning.Services;
using TournoisPlanning.ViewModels;

namespace TournoisPlanning.Views
{
    public partial class GestionEquipesDialog : UserControl
    {
        public GestionEquipesDialog(int tournoiId)
        {
            InitializeComponent();
            DataContext = new EquipeViewModel(tournoiId);
        }
    }
}