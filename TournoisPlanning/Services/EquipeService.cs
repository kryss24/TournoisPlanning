using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using TournoisPlanning.Models;
using TournoisPlanning.Config; // Supposant que DBConn est dans ce namespace

namespace TournoisPlanning.Services
{
    public class EquipeService : IEquipeService
    {
        private readonly DBConn _dbConn;

        public EquipeService()
        {
            _dbConn = new DBConn("127.0.0.1", "adminTM", "firstproject", "tournoisManagement"); // Initialiser la connexion à la base de données
        }

        public void CreerEquipe(Equipes equipe)
        {
            try
            {
                string query = @"
                    INSERT INTO Equipes (TournoiId, Nom, Contact, Email, Telephone)
                    VALUES (@TournoiId, @Nom, @Contact, @Email, @Telephone);
                    SELECT LAST_INSERT_ID();"; // Récupérer l'ID généré

                var parameters = new List<MySqlParameter>
                {
                    new MySqlParameter("@TournoiId", equipe.TournoiId),
                    new MySqlParameter("@Nom", equipe.Nom ?? (object)DBNull.Value),
                    new MySqlParameter("@Contact", equipe.Contact ?? (object)DBNull.Value),
                    new MySqlParameter("@Email", equipe.Email ?? (object)DBNull.Value),
                    new MySqlParameter("@Telephone", equipe.Telephone ?? (object)DBNull.Value)
                };

                // Exécuter la requête et récupérer l'identifiant généré
                object result = _dbConn.ExecuteScalar(query, parameters.ToArray());

                // Assigner l'ID généré à l'équipe
                if (result != null && result != DBNull.Value)
                {
                    equipe.Id = Convert.ToInt32(result);
                }
                ITournoiService _tournoiService = ServiceLocator.GetService<ITournoiService>();
                _tournoiService.RefreshTournaments();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors de la création de l'équipe: {ex.Message}", ex);
            }
        }

        public void SupprimerEquipe(int id)
        {
            try
            {
                string query = "DELETE FROM Equipes WHERE Id = @Id";
                var parameters = new[] { new MySqlParameter("@Id", id) };

                _dbConn.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors de la suppression de l'équipe: {ex.Message}", ex);
            }
        }

        public Equipes ObtenirEquipe(int id)
        {
            try
            {
                string query = @"
                    SELECT Id, TournoiId, Nom, Contact, Email, Telephone
                    FROM Equipes
                    WHERE Id = @Id";

                var parameters = new[] { new MySqlParameter("@Id", id) };

                DataTable dataTable = _dbConn.ExecuteQuery(query, parameters);

                if (dataTable.Rows.Count > 0)
                {
                    return MapEquipeFromDataRow(dataTable.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors de la récupération de l'équipe: {ex.Message}", ex);
            }
        }

        public List<Equipes> ObtenirToutesEquipes()
        {
            try
            {
                string query = @"
                    SELECT Id, TournoiId, Nom, Contact, Email, Telephone
                    FROM Equipes";

                DataTable dataTable = _dbConn.ExecuteQuery(query);

                return MapEquipesFromDataTable(dataTable);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors de la récupération des équipes: {ex.Message}", ex);
            }
        }

        public List<Equipes> ObtenirEquipesParTournoi(int tournoiId)
        {
            try
            {
                string query = @"
                    SELECT Id, TournoiId, Nom, Contact, Email, Telephone
                    FROM Equipes
                    WHERE TournoiId = @TournoiId";

                var parameters = new[] { new MySqlParameter("@TournoiId", tournoiId) };

                DataTable dataTable = _dbConn.ExecuteQuery(query, parameters);

                return MapEquipesFromDataTable(dataTable);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erreur lors de la récupération des équipes du tournoi: {ex.Message}", ex);
            }
        }

        private Equipes MapEquipeFromDataRow(DataRow row)
        {
            return new Equipes
            {
                Id = Convert.ToInt32(row["Id"]),
                TournoiId = Convert.ToInt32(row["TournoiId"]),
                Nom = row["Nom"] == DBNull.Value ? null : row["Nom"].ToString(),
                Contact = row["Contact"] == DBNull.Value ? null : row["Contact"].ToString(),
                Email = row["Email"] == DBNull.Value ? null : row["Email"].ToString(),
                Telephone = row["Telephone"] == DBNull.Value ? null : row["Telephone"].ToString()
            };
        }

        private List<Equipes> MapEquipesFromDataTable(DataTable dataTable)
        {
            List<Equipes> equipes = new List<Equipes>();

            foreach (DataRow row in dataTable.Rows)
            {
                equipes.Add(MapEquipeFromDataRow(row));
            }

            return equipes;
        }
    }
}