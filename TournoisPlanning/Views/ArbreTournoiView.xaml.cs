using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TournoisPlanning;
using TournoisPlanning.ViewModels;

namespace TournoisPlanning.Views
{
    public partial class ArbreTournoiView : UserControl
    {
        private const double BoxWidth = 100;
        private const double BoxHeight = 40;
        private const double HorizontalSpacing = 80;
        private const double VerticalSpacing = 40;

        public ArbreTournoiView(Tournoi tournoi)
        {
            InitializeComponent();
            Loaded += ArbreTournoiView_Loaded;
        }

        private void ArbreTournoiView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is TournoiDetailsViewModel vm)
            {
                MatchCanvas.Children.Clear();
                DrawTree(vm.RootMatches);
            }
        }

        private void DrawTree(IEnumerable<Match> rootMatches)
        {
            double y = 20;

            foreach (var match in rootMatches)
            {
                DrawMatchBranch(match, 0, y);
                y += (BoxHeight + VerticalSpacing) * GetMatchDepth(match);
            }
        }
        //private void DrawTreeMirror(Match racineGauche, Match racineDroite, Match finale)
        //{
        //    double centerX = MatchCanvas.ActualWidth / 2;
        //    double centerY = MatchCanvas.ActualHeight / 2;

        //    double maxDepth = Math.Max(GetMaxDepth(racineGauche), GetMaxDepth(racineDroite));
        //    double branchHeight = (BoxHeight + VerticalSpacing) * Math.Pow(2, maxDepth - 1);

        //    double startYGauche = centerY - branchHeight / 2;
        //    double startYDroite = centerY - branchHeight / 2;

        //    DrawBranch(racineGauche, centerX - HorizontalSpacing - BoxWidth, startYGauche, -1);
        //    DrawBranch(racineDroite, centerX + HorizontalSpacing, startYDroite, 1);

        //    // Dessiner la finale au centre
        //    var finaleBox = CreateMatchBox(finale, centerX - BoxWidth / 2, centerY - BoxHeight / 2);
        //    TournamentCanvas.Children.Add(finaleBox);

        //    DrawLineToFinal(racineGauche, finale, centerX - BoxWidth / 2, centerY);
        //    DrawLineToFinal(racineDroite, finale, centerX + BoxWidth / 2, centerY);
        //}


        private int DrawMatchBranch(Match match, int level, double y)
        {
            double x = level * (BoxWidth + HorizontalSpacing);
            var matchBox = CreateMatchBox(match, x, y);
            MatchCanvas.Children.Add(matchBox);

            int subtreeHeight = 1;

            if (match.MatchSuivant != null)
            {
                double nextX = (level + 1) * (BoxWidth + HorizontalSpacing);
                double nextY = y + (BoxHeight + VerticalSpacing) / 2;

                var line = new Line
                {
                    X1 = x + BoxWidth,
                    Y1 = y + BoxHeight / 2,
                    X2 = nextX,
                    Y2 = nextY,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                MatchCanvas.Children.Add(line);

                subtreeHeight = DrawMatchBranch(match.MatchSuivant, level + 1, nextY - BoxHeight / 2);
            }

            return subtreeHeight;
        }

        private Border CreateMatchBox(Match match, double x, double y)
        {
            var border = new Border
            {
                Width = BoxWidth,
                Height = BoxHeight,
                Background = new SolidColorBrush(Color.FromRgb(200, 230, 255)),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(5),
                Child = new TextBlock
                {
                    Text = $"{match.Equipe1} vs {match.Equipe2}",
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap
                }
            };

            Canvas.SetLeft(border, x);
            Canvas.SetTop(border, y);

            return border;
        }

        private int GetMatchDepth(Match match)
        {
            if (match.MatchSuivant == null) return 1;
            return 1 + GetMatchDepth(match.MatchSuivant);
        }
    }
}
