using System.Windows;

namespace TournoisPlanning
{
    //public class Match
    //{
    //    public string Equipe1 { get; set; }
    //    public string Equipe2 { get; set; }
    //    public string Gagnant { get; set; }
    //    public Match? MatchSuivant { get; set; }
    //    public Point Position { get; set; }
    //}
    public class Match
    {
        public string Equipe1 { get; set; } = string.Empty;
        public string Equipe2 { get; set; } = string.Empty;
        public Match? MatchSuivant { get; set; }
        public int Round { get; set; } // 1 pour premier tour, 2 pour quart, etc.
        public string Statut { get; set; } // Exemple : "En cours", "Terminé"
    }

}
