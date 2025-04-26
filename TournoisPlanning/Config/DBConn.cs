using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TournoisPlanning.Config
{
    public class DBConn
    {
        private string connectionString;
        private MySqlConnection connection;
        public DBConn(string server, string username, string password, string database)
        {
            connectionString = $"Server={server};Database={database};UserID={username};Password={password};";
            connection = new MySqlConnection(connectionString);
        }
        public void OpenConnection()
        {
            try
            {
                connection.Open();
            }
            catch (MySqlException ex)
            {
                throw new Exception("Erreur de connexion a la database", ex);
            }
        }
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(this.connectionString);
        }
        public void CloseConnection()
        {
            try
            {
                connection.Close();
            }
            catch (MySqlException ex)
            {
                throw new Exception("Erreur lors de la fermeture de la connexion", ex);
            }
        }
        public MySqlDataReader ExecuteReader(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            return command.ExecuteReader();
        }
        public int ExecuteNonQuery(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            return command.ExecuteNonQuery();
        }
        public object ExecuteScalar(string query)
        {
            MySqlCommand commamd = new MySqlCommand(query, connection);
            return commamd.ExecuteScalar();
        }

        public int VerifiedUser(string username, string password)
        {
            string query = "SELECT COUNT(*) FROM users WHERE Username=@username AND Password=@password";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            int count = Convert.ToInt32(command.ExecuteScalar());
            return count;
        }

        //Add Config
        public DataTable ExecuteQuery(string query, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection connection = new MySqlConnection(this.connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                DataTable dataTable = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                try
                {
                    connection.Open();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Erreur lors de l'exécution de la requête: {ex.Message}", ex);
                }
            }
        }

        public int ExecuteNonQuery(string query, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection connection = new MySqlConnection(this.connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Erreur lors de l'exécution de la commande: {ex.Message}", ex);
                }
            }
        }

        public object ExecuteScalar(string query, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection connection = new MySqlConnection(this.connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                try
                {
                    connection.Open();
                    return command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Erreur lors de l'exécution de la commande: {ex.Message}", ex);
                }
            }
        }
    }
}
