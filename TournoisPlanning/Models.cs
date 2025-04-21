using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TournoisPlanning
{
    public class Tournoi
    {
        public string Nom { get; set; }
        public string Type { get; set; } // Exemple : "Élimination directe", "Poules"
        public int NombreEquipes { get; set; }
        public string Statut { get; set; } // Exemple : "En cours", "Terminé"
        public Match? RacineGauche { get; set; }
        public Match? RacineDroite { get; set; }
        public List<Match> Matches { get; set; }
        // Exemple de méthode GetMatchEnCours
        public Match GetMatchEnCours()
        {
            return Matches.FirstOrDefault(m => m.Statut == "En cours");
        }

        // Exemple de méthode GetProchainMatch
        public Match GetProchainMatch()
        {
            return Matches.FirstOrDefault(m => m.Statut == "À venir");
        }
    }
}
