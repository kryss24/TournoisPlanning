using System;
using System.Windows;
using System.Windows.Controls;
using TournoisPlanning.ViewModels;

namespace TournoisPlanning.Views
{
    public partial class CreateTournamentForm : UserControl
    {
        private CreateTournamentViewModel _viewModel;

        public CreateTournamentForm()
        {
            InitializeComponent();
            _viewModel = new CreateTournamentViewModel();
            this.DataContext = _viewModel;
        }
    }
}