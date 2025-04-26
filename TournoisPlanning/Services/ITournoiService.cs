using System;
using System.Collections.Generic;
using TournoisPlanning.Models;

namespace TournoisPlanning.Services
{
    // Interface pour le service de gestion des tournois
    public interface ITournoiService
    {
        int CreerTournoi(Tournoi tournoi);
        void GenererPlanningAutomatique(int tournoiId);
        Tournoi ObtenirTournoi(int id);
        List<Tournoi> ObtenirTousTournois();
        void ModifierTournoi(Tournoi tournoi);
        void SupprimerTournoi(int id);
        void RefreshTournaments();
    }

    // Interface pour le service de navigation
    public interface INavigationService
    {
        void NavigateToTournamentDetails(int tournoiId);
        void NavigateToTournamentsList();
        void GoBack();
    }

    // Service Locator simple (à remplacer par un container IoC dans une application plus complexe)
    public static class ServiceLocator
    {

        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void RegisterService<T>(T service) where T : class
        {
            _services[typeof(T)] = service;
        }

        public static T GetService<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }
            throw new InvalidOperationException($"Service of type {typeof(T).Name} is not registered");
        }

        public static void Clear()
        {
            _services.Clear();
        }
    }
}