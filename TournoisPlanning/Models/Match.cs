using System.Windows;

namespace TournoisPlanning.Models
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
        public int Id { get; set; }
        public required Equipes Equipe1 { get; set; }
        public int ScoreEquipe1 { get; set; }
        public required Equipes Equipe2 { get; set; }
        public int ScoreEquipe2 {  get; set; }
        public int TournoiId { get; set; }
        public required string Lieu { get; set; }
        public bool EstTermine { get; set; }
        public Match? MatchSuivant { get; set; }
        public int Round { get; set; } // 1 pour premier tour, 2 pour quart, etc.
        public string Statut { get; set; } // Exemple : "En cours", "Terminé"
        public string Score => $"{ScoreEquipe1} - {ScoreEquipe2}";
        public DateTime? Date { get; set; }
    }

}
