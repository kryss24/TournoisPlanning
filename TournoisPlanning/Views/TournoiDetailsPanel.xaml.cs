﻿using System;
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
using TournoisPlanning.ViewModels;
using TournoisPlanning.Views;
using TournoisPlanning.Models;

namespace TournoisPlanning.Views
{
    /// <summary>
    /// Logique d'interaction pour TournoiDetailsPanel.xaml
    /// </summary>
    public partial class TournoiDetailsPanel : UserControl
    {
        public TournoiDetailsPanel()
        {
            InitializeComponent();
        }
        public void SetTournoi(Tournoi tournoi)
        {
            // Définir le DataContext à une instance de TournoiDetailsViewModel
            DataContext = new TournoiDetailsViewModel(tournoi);
        }
    }
}
