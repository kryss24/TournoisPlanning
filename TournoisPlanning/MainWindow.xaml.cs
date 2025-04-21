using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TournoisPlanning;

namespace TournoisPlanning
{
    public partial class MainWindow : Window
    {
        //private TournamentManager manager;

        public MainWindow()
        {
            InitializeComponent();
            //manager = new TournamentManager();
            //DrawTournament(manager.RootMatches);
        }

        private void StartDashBoard_Click(object sender, RoutedEventArgs e)
        {
            var dashboard = new Dashboard();
            dashboard.Show();
        }

        //private void DrawTournament(List<Match> matchesPremierTour)
        //{
        //    TournamentCanvas.Children.Clear();

        //    double matchWidth = 120;
        //    double matchHeight = 40;
        //    double verticalSpacing = 40;
        //    double horizontalSpacing = 200;

        //    Dictionary<int, List<Match>> levels = new();
        //    Dictionary<Match, int> matchLevels = new();

        //    // Organiser par niveaux
        //    Queue<(Match match, int level)> queue = new();
        //    foreach (var match in matchesPremierTour)
        //        queue.Enqueue((match, 0));

        //    while (queue.Count > 0)
        //    {
        //        var (match, level) = queue.Dequeue();
        //        matchLevels[match] = level;

        //        if (!levels.ContainsKey(level))
        //            levels[level] = new List<Match>();
        //        if (!levels[level].Contains(match))
        //            levels[level].Add(match);

        //        if (match.MatchSuivant != null)
        //            queue.Enqueue((match.MatchSuivant, level + 1));
        //    }

        //    // Dessin des matchs
        //    foreach (var kvp in levels)
        //    {
        //        int lvl = kvp.Key;
        //        var matches = kvp.Value;
        //        double startY = 50 + ((TournamentCanvas.ActualHeight == 0 ? 600 : TournamentCanvas.ActualHeight) - ((matchHeight + verticalSpacing) * matches.Count)) / 2;

        //        for (int i = 0; i < matches.Count; i++)
        //        {
        //            var match = matches[i];
        //            double x = lvl * horizontalSpacing + 50;
        //            double y = i * (matchHeight + verticalSpacing) + startY;

        //            var rect = new Rectangle
        //            {
        //                Width = matchWidth,
        //                Height = matchHeight,
        //                Fill = Brushes.LightGray,
        //                Stroke = Brushes.Black
        //            };
        //            Canvas.SetLeft(rect, x);
        //            Canvas.SetTop(rect, y);
        //            TournamentCanvas.Children.Add(rect);

        //            string labelText = $"{match.Equipe1 ?? "?"} vs {match.Equipe2 ?? "?"}";
        //            if (match.Gagnant != null)
        //                labelText += $"\n→ {match.Gagnant}";

        //            var btn = new Button
        //            {
        //                Content = labelText,
        //                Width = matchWidth,
        //                Height = matchHeight,
        //                Background = Brushes.Transparent,
        //                BorderThickness = new Thickness(0),
        //                Tag = match
        //            };
        //            btn.Click += Match_Click;
        //            Canvas.SetLeft(btn, x);
        //            Canvas.SetTop(btn, y);
        //            TournamentCanvas.Children.Add(btn);

        //            match.Position = new Point(x + matchWidth, y + (matchHeight / 2));

        //            // Ligne vers match suivant
        //            if (match.MatchSuivant != null)
        //            {
        //                double tx = (lvl + 1) * horizontalSpacing + 50;

        //                // Calculer le point médian entre les deux matchs connectés
        //                var nextMatchIndex = levels[lvl + 1].FindIndex(m => m == match.MatchSuivant);
        //                double ty1 = y + matchHeight / 2; // Point de départ de la ligne
        //                double ty2 = nextMatchIndex * (matchHeight + verticalSpacing) + startY + (matchHeight / 2); // Centre du match suivant

        //                // Calculer le milieu entre les deux matchs
        //                double midY = ((ty1 + ty2)) / 2; // 20 unités au-dessus du milieu
        //                MessageBox.Show((nextMatchIndex % 2 == 0).ToString());
        //                // Dessiner les lignes
        //                var line1 = new Line
        //                {
        //                    X1 = x + matchWidth,
        //                    Y1 = ty1,
        //                    X2 = tx - horizontalSpacing / 2 + matchWidth,
        //                    Y2 = midY,
        //                    Stroke = Brushes.Black,
        //                    StrokeThickness = 2
        //                };
        //                TournamentCanvas.Children.Add(line1);

        //                //var line2 = new Line
        //                //{
        //                //    X1 = tx - horizontalSpacing / 2,
        //                //    Y1 = midY,
        //                //    X2 = tx,
        //                //    Y2 = ty2,
        //                //    Stroke = Brushes.Black,
        //                //    StrokeThickness = 2
        //                //};
        //                //TournamentCanvas.Children.Add(line2);
        //            }
        //        }
        //    }
        //}

        //private void Match_Click(object sender, RoutedEventArgs e)
        //{
        //    var button = sender as Button;
        //    if (button == null) return;

        //    var match = button.Tag as Match;
        //    if (match == null || match.Equipe1 == null || match.Equipe2 == null) return;

        //    var popup = new WinnerSelectionWindow(match.Equipe1, match.Equipe2)
        //    {
        //        Owner = this
        //    };

        //    if (popup.ShowDialog() == true)
        //    {
        //        match.Gagnant = popup.GagnantChoisi;

        //        if (match.MatchSuivant != null)
        //        {
        //            if (string.IsNullOrEmpty(match.MatchSuivant.Equipe1))
        //                match.MatchSuivant.Equipe1 = match.Gagnant;
        //            else if (string.IsNullOrEmpty(match.MatchSuivant.Equipe2))
        //                match.MatchSuivant.Equipe2 = match.Gagnant;
        //        }

        //        DrawTournament(manager.RootMatches);
        //    }
        //}

    }
}
