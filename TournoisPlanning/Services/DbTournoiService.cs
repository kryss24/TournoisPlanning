using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using TournoisPlanning.Config;
using TournoisPlanning.Models;
using TournoisPlanning.ViewModels;
using TournoisPlanning.Views;

namespace TournoisPlanning.Services
{
    public class DbTournoiService : ITournoiService
    {
        private readonly DBConn _dbConnection;

        public DbTournoiService(DBConn dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public int CreerTournoi(Tournoi tournoi)
        {
            string sql = @"
                INSERT INTO Tournois (
                    Nom, Type, DateDebut, NombreEquipes, FrequenceMatch, 
                    DureeMatch, Lieu, Description, Prix, PlanificationAutomatique
                ) VALUES (
                    @Nom, @Type, @DateDebut, @NombreEquipes, @FrequenceMatch, 
                    @DureeMatch, @Lieu, @Description, @Prix, @PlanificationAutomatique
                );
                SELECT LAST_INSERT_ID();";

            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Nom", tournoi.Nom);
                    command.Parameters.AddWithValue("@Type", (int)tournoi.Type);
                    command.Parameters.AddWithValue("@DateDebut", tournoi.DateDebut);
                    command.Parameters.AddWithValue("@NombreEquipes", tournoi.NombreEquipes);
                    command.Parameters.AddWithValue("@FrequenceMatch", (int)tournoi.FrequenceMatch);
                    command.Parameters.AddWithValue("@DureeMatch", tournoi.DureeMatch);
                    command.Parameters.AddWithValue("@Lieu", tournoi.Lieu);
                    command.Parameters.AddWithValue("@Description", tournoi.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Prix", tournoi.Prix ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PlanificationAutomatique", tournoi.PlanificationAutomatique);
                    command.Parameters.AddWithValue("@Saison", tournoi.Saison);

                    tournoi.Id = Convert.ToInt32(command.ExecuteScalar());
                    return tournoi.Id;
                }
            }
        }
        public Equipes AjouterEquipe(Equipes equipe)
        {
            string sql = @"
                INSERT INTO Equipes (Nom, Contact, Email, Telephone, TournoiId) 
                VALUES (@Nom, @Contact, @Email, @Telephone, @TournoiId);
                SELECT LAST_INSERT_ID();";
            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Nom", equipe.Nom);
                    command.Parameters.AddWithValue("@Contact", equipe.Contact);
                    command.Parameters.AddWithValue("@Email", equipe.Email);
                    command.Parameters.AddWithValue("@Telephone", equipe.Telephone);
                    command.Parameters.AddWithValue("@TournoiId", equipe.TournoiId);
                    equipe.Id = Convert.ToInt32(command.ExecuteScalar());
                    return equipe;
                }
            }
        }
        public Tournoi ObtenirTournoi(int id)
        {
            string sql = "SELECT * FROM Tournois WHERE Id = @Id";

            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapTournoiFromReader(reader);
                        }
                    }
                }
            }

            return null;
        }

        public List<Tournoi> ObtenirTousTournois()
        {
            string sql = "SELECT * FROM Tournois ORDER BY DateDebut DESC";
            var tournois = new List<Tournoi>();

            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tournois.Add(MapTournoiFromReader(reader));
                        }
                    }
                }
            }

            return tournois;
        }

        public void ModifierTournoi(Tournoi tournoi)
        {
            string sql = @"
                UPDATE Tournois SET 
                    Nom = @Nom, 
                    Type = @Type, 
                    DateDebut = @DateDebut, 
                    NombreEquipes = @NombreEquipes, 
                    FrequenceMatch = @FrequenceMatch,
                    DureeMatch = @DureeMatch, 
                    Lieu = @Lieu, 
                    Description = @Description, 
                    Prix = @Prix,
                    PlanificationAutomatique = @PlanificationAutomatique
                WHERE Id = @Id";

            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", tournoi.Id);
                    command.Parameters.AddWithValue("@Nom", tournoi.Nom);
                    command.Parameters.AddWithValue("@Type", (int)tournoi.Type);
                    command.Parameters.AddWithValue("@DateDebut", tournoi.DateDebut);
                    command.Parameters.AddWithValue("@NombreEquipes", tournoi.NombreEquipes);
                    command.Parameters.AddWithValue("@FrequenceMatch", (int)tournoi.FrequenceMatch);
                    command.Parameters.AddWithValue("@DureeMatch", tournoi.DureeMatch);
                    command.Parameters.AddWithValue("@Lieu", tournoi.Lieu);
                    command.Parameters.AddWithValue("@Description", tournoi.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Prix", tournoi.Prix ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PlanificationAutomatique", tournoi.PlanificationAutomatique);

                    if (command.ExecuteNonQuery() == 0)
                    {
                        throw new KeyNotFoundException($"Tournoi avec ID {tournoi.Id} non trouvé");
                    }
                }
            }
        }

        public void SupprimerTournoi(int id)
        {
            string sql = "DELETE FROM Tournois WHERE Id = @Id";

            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        private List<Match> ObtenirMatchsPourTournoi(int tournoiId)
        {
            var matchs = new List<Match>();

            // SQL optimisé: sélectionne uniquement les colonnes nécessaires et utilise des alias plus courts
            string sql = @"
        SELECT 
            m.Id,
            m.DateMatch,
            m.ScoreEquipe1,
            m.ScoreEquipe2,
            m.Statut,
            m.Terrain,
            ea.Id AS Equipe1Id,
            ea.Nom AS Equipe1Nom,
            ea.Email AS Equipe1Email,
            ea.Telephone AS Equipe1Tel,
            ea.Contact AS Equipe1Contact,
            eb.Id AS Equipe2Id,
            eb.Nom AS Equipe2Nom,
            eb.Email AS Equipe2Email,
            eb.Telephone AS Equipe2Tel,
            eb.Contact AS Equipe2Contact
        FROM Matchs m
        JOIN Equipes ea ON m.Equipe1Id = ea.Id
        JOIN Equipes eb ON m.Equipe2Id = eb.Id
        WHERE m.TournoiId = @TournoiId
        ORDER BY m.DateMatch;
    ";

            try
            {
                using (var connection = _dbConnection.GetConnection())
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@TournoiId", tournoiId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Création des équipes avec des noms de colonnes plus clairs
                            var equipe1 = new Equipes
                            {
                                Id = Convert.ToInt32(reader["Equipe1Id"]),
                                Nom = reader["Equipe1Nom"].ToString(),
                                Email = reader["Equipe1Email"].ToString(),
                                Telephone = reader["Equipe1Tel"].ToString(),
                                Contact = reader["Equipe1Contact"].ToString(),
                            };

                            var equipe2 = new Equipes
                            {
                                Id = Convert.ToInt32(reader["Equipe2Id"]),
                                Nom = reader["Equipe2Nom"].ToString(),
                                Email = reader["Equipe2Email"].ToString(),
                                Telephone = reader["Equipe2Tel"].ToString(),
                                Contact = reader["Equipe2Contact"].ToString(),
                            };

                            // Simplification de la gestion des scores null
                            int? scoreEquipe1 = reader["ScoreEquipe1"] != DBNull.Value ?
                                                Convert.ToInt32(reader["ScoreEquipe1"]) : null;
                            int? scoreEquipe2 = reader["ScoreEquipe2"] != DBNull.Value ?
                                                Convert.ToInt32(reader["ScoreEquipe2"]) : null;

                            // Création du match
                            matchs.Add(new Match
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Equipe1 = equipe1,
                                Equipe2 = equipe2,
                                Date = Convert.ToDateTime(reader["DateMatch"]),
                                ScoreEquipe1 = (int)scoreEquipe1,
                                ScoreEquipe2 = (int)scoreEquipe2,
                                Statut = reader["Statut"].ToString(),
                                Lieu = reader["Terrain"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Gestion des erreurs
                Console.WriteLine($"Erreur lors de la récupération des matchs: {ex.Message}");
                // Ou log l'erreur avec un système de logging
            }

            return matchs;
        }

        public List<Equipes> ObtenirEquipesPourTournoi(int tournoiId)
        {
            var equipe = new List<Equipes>();
            string sql = @"SELECT * FROM equipes WHERE TournoiId = @TournoiId";

            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@TournoiId", tournoiId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            equipe.Add(new Equipes
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                TournoiId = Convert.ToInt32(reader["TournoiId"]),
                                Nom = reader["Nom"].ToString(),
                                Contact = reader["Contact"].ToString(),
                                Email = reader["Email"].ToString(),
                                Telephone = reader["Telephone"].ToString(),
                            });
                        }
                    }
                }
            }

            return equipe;
        }

        public void GenererPlanningAutomatique(int tournoiId)
        {
            var tournoi = ObtenirTournoi(tournoiId);
            if (tournoi == null)
                throw new ArgumentException($"Tournoi avec ID {tournoiId} non trouvé");

            // Ajoute ici la logique de génération automatique
        }

        private Tournoi MapTournoiFromReader(IDataReader reader)
        {
            return new Tournoi
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nom = reader["Nom"].ToString(),
                Type = (TypeTournoi)Convert.ToInt32(reader["Type"]),
                DateDebut = Convert.ToDateTime(reader["DateDebut"]),
                NombreEquipes = Convert.ToInt32(reader["NombreEquipes"]),
                FrequenceMatch = (FrequenceMatch)Convert.ToInt32(reader["FrequenceMatch"]),
                DureeMatch = Convert.ToInt32(reader["DureeMatch"]),
                Lieu = reader["Lieu"].ToString(),
                Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                Prix = reader["Prix"] == DBNull.Value ? null : reader["Prix"].ToString(),
                PlanificationAutomatique = Convert.ToBoolean(reader["PlanificationAutomatique"]),
                Saison = reader["Saison"].ToString(),
                Matches = ObtenirMatchsPourTournoi(Convert.ToInt32(reader["Id"])),
                Equipes = ObtenirEquipesPourTournoi(Convert.ToInt32(reader["Id"])),
            };
        }
        public void RefreshTournaments()
        {
            MessageBox.Show("refresh");
            // Récupérer tous les tournois depuis la base de données
            var tournois = ObtenirTousTournois();

            // Mettre à jour la collection observable dans le ViewModel
            Application.Current.Dispatcher.Invoke(() =>
            {
                var dashboardViewModel = ServiceLocator.GetService<Dashboard>();
                if (dashboardViewModel != null)
                {
                    dashboardViewModel.Tournois.Clear();
                    foreach (var tournoi in tournois)
                    {
                        dashboardViewModel.Tournois.Add(tournoi);
                    }
                    foreach (var tournois in dashboardViewModel.Tournois)
                    {
                        tournois.InitialiserProchainMatch();
                    }
                }
            });
        }
        public event EventHandler TournamentsChanged;

        protected virtual void OnTournamentsChanged()
        {
            TournamentsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
