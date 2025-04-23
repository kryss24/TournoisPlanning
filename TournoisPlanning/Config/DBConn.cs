using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TournoisPlanning.Config
{
    class DBConn
    {
        private string connectionString;
        private MySqlConnection connection;
        public DBConn(string server, string username, string password, string database)
        {
            connectionString = $"Server={server};Database={database};UserID={username};Password={password};";
            connection = new MySqlConnection(connectionString) ;
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
        public void CloseConnection() {
            try
            {
                connection.Close();
            }
            catch (MySqlException ex) {
                throw new Exception("Erreur lors de la fermeture de la connexion", ex);
            }
        }
        public MySqlDataReader ExecuteReader(string query)
        {
            MySqlCommand command = new MySqlCommand(query, connection);
            return command.ExecuteReader();
        }
        public int ExecuteNonQuery(string query) { 
            MySqlCommand command = new MySqlCommand(query, connection) ;
            return command.ExecuteNonQuery();
        }
        public object ExecuteScalar(string query) { 
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
    }
}
