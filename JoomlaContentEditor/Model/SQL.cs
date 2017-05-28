using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JoomlaContentEditor.Model
{
    public static class SQL
    {
        public static MySqlConnection SqlConnection;

        public static async Task<bool> ConnectToDatabase()
        {
            using (var _sqlConnection = new MySqlConnection())
            {
                MySqlConnectionStringBuilder hash = new MySqlConnectionStringBuilder();
                hash.UserID = "root";
                hash.Password = "";
                hash.Server = "localhost";
                hash.Database = "joomla";

                _sqlConnection.ConnectionString = hash.ConnectionString;

                try
                {
                    await _sqlConnection.OpenAsync();

                    if (_sqlConnection.State == System.Data.ConnectionState.Open)
                    {
                        SqlConnection = _sqlConnection;
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas łączenia się z bazą danych" + Environment.NewLine + ex.Message);
                    return false;
                }
            }
        }

        public static async Task<string> GetPlainHTMLOfChanges()
        {
            string query = "SELECT `introtext` FROM `jm_content` WHERE `id` = 1;";

            using (var command = new MySqlCommand(query, SqlConnection))
            {
                try
                {
                    await SqlConnection.OpenAsync();

                    var returnPlainHtml = ((await command.ExecuteScalarAsync()) as string);

                    await SqlConnection.CloseAsync();

                    return returnPlainHtml;
                }
                catch(Exception ex)
                {
                    await SqlConnection.CloseAsync();
                    MessageBox.Show("Błąd podczas pobierania raw HTML" + Environment.NewLine + ex.Message);
                    return null;
                }
            }
        }

        public static async Task<bool> SendPlainHTMLOfChanges(string @changes)
        {
            string @query = @"UPDATE `jm_content` SET introtext = '" + @changes + @"' WHERE `id` = 1;";

            using (var command = new MySqlCommand(query, SqlConnection))
            {
                try
                {
                    await SqlConnection.OpenAsync();

                    await command.ExecuteNonQueryAsync();

                    await SqlConnection.CloseAsync();

                    return true;
                }
                catch(Exception ex)
                {
                    await SqlConnection.CloseAsync();
                    MessageBox.Show("Błąd podczas wysyłania żądania raw HTML" + Environment.NewLine + ex.Message);
                    return false;
                }
            }
        }
    }
}
