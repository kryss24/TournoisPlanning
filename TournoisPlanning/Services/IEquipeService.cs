using System.Collections.Generic;
using TournoisPlanning.Models;

namespace TournoisPlanning.Services
{
    public interface IEquipeService
    {
        void CreerEquipe(Equipes equipe);
        Equipes ObtenirEquipe(int id);
        List<Equipes> ObtenirToutesEquipes();
        List<Equipes> ObtenirEquipesParTournoi(int tournoiId);
        void SupprimerEquipe(int id);
    }
}