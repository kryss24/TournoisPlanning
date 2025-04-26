using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournoisPlanning.Models
{
    public enum TypeTournoi
    {
        Knockout,
        Poules,
        Mixed
    }

    public enum FrequenceMatch
    {
        Daily,
        Weekly,
        Custom
    }
    public class Tournoi
    {
        public int Id { get; set; }
        public required string Nom { get; set; }
        public TypeTournoi Type { get; set; } // Exemple : "Élimination directe", "Poules"
        public DateTime DateDebut { get; set; }
        public FrequenceMatch FrequenceMatch { get; set; }
        public int DureeMatch { get; set; }
        public required string Lieu { get; set; }
        public string? Description { get; set; }
        public string? Prix { get; set; }
        public bool PlanificationAutomatique { get; set; }
        public int NombreEquipes { get; set; }
        public string Statut { get; set; } // Exemple : "En cours", "Terminé"
        public int Completion { get; set; }
        public required string Saison { get; set; }
        
        public List<Match> Matches { get; set; }
        public List<Equipes> Equipes { get; set; }
        public Match ProchainMatch { get; set; }
        // Cette propriété retourne une couleur différente selon l'avancement du tournoi
        public string ProgressColor
        {
            get
            {
                if (Completion >= 80) return "#4CAF50"; // Vert pour un avancement élevé
                if (Completion >= 60) return "#8C52FF"; // Violet pour un avancement moyen
                return "#FF5722"; // Orange pour un avancement faible
            }
        }
        // Exemple de méthode GetMatchEnCours
        public Match GetMatchEnCours()
        {
            return Matches.FirstOrDefault(m => m.Statut == "En cours");
        }

        // Exemple de méthode GetProchainMatch
        public Match GetProchainMatch()
        {
            Match match = Matches.FirstOrDefault(m => m.Statut == "À venir");
            ProchainMatch = match;
            return match;
        }
        public void InitialiserProchainMatch()
        {
            //if (Matches.Count >= 2)
            //{
            //    var match = Matches[1];
            //    ProchainMatch = match;
            //}
            ProchainMatch = GetProchainMatch();
        }
    }

}
