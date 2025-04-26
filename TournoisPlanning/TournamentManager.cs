using System.Collections.Generic;
using TournoisPlanning.Models;

namespace TournoisPlanning
{
    public class TournamentManager
    {
        public List<Match> RootMatches { get; private set; }

        public TournamentManager()
        {
            //GenerateTournament();
        }

        //public void GenerateTournament()
        //{
        //    var m1 = new Match { Equipe1 = "Équipe 1", Equipe2 = "Équipe 2" };
        //    var m2 = new Match { Equipe1 = "Équipe 3", Equipe2 = "Équipe 4" };
        //    var m3 = new Match { Equipe1 = "Équipe 5", Equipe2 = "Équipe 6" };
        //    var m4 = new Match { Equipe1 = "Équipe 7", Equipe2 = "Équipe 8" };

        //    var m5 = new Match();
        //    var m6 = new Match();
        //    var m7 = new Match(); // Finale

        //    m1.MatchSuivant = m5;
        //    m2.MatchSuivant = m5;
        //    m3.MatchSuivant = m6;
        //    m4.MatchSuivant = m6;

        //    m5.MatchSuivant = m7;
        //    m6.MatchSuivant = m7;

        //    RootMatches = new List<Match> { m1, m2, m3, m4 };
        //}
    }
}
